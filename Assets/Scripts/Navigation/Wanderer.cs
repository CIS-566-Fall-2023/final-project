using BinaryPartition;
using Geom;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Navigation {

    public class Wanderer : MonoBehaviour {
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public bool isMoving = false;
        public bool isInRoom = false;
        private PathFinder pathFinder;
        public Graph navGraph;
        public Stack<EdgeInfo> path;
        public EdgeInfo startEdge { get; set; }
        public EdgeInfo currEdge { get; set; }
        public EdgeInfo endEdge { get; set; }
        public bool justEntered = false;

        // MOVEMENT
        private float lerpDuration = 1.5f; // You can adjust the duration to control the speed of movement
        private float lerpDurationRoom= 10.0f;
        private float lerpStartTime;

        // yum BEZIER STUFF
        private List<Vector2> controlPoints;
        private float movementDuration;

        public void Initialize(Graph navGraph, EdgeInfo start, EdgeInfo end, PathFinder pathFinder) {

            this.navGraph = navGraph;
            this.Position = start.Curve.Point(0);
            this.startEdge = start;
            this.endEdge = end;
            this.pathFinder = pathFinder;
            this.path = pathFinder.FindPath(start, end);
        }


        void Update()
        {
            this.GetComponent<FootprintTrail>().pos = this.Position;
            if (isInRoom) return;
            if (!isMoving  && path.Count > 0) {
                currEdge = path.Pop();
                StartCoroutine(MoveToTarget(currEdge.Curve));
                if (currEdge.Tag == EdgeTag.Doorway && justEntered == false)
                {
                    isInRoom = true;
                    StartCoroutine(EnterRoom(currEdge));
                }
                else
                {
                    justEntered = false;
                    StartCoroutine(MoveToTarget(currEdge.Curve)); 
                }
                  
            }
            else if (path.Count == 0) {
                endEdge = navGraph.GetRandomEdge();
                path = pathFinder.FindPath(currEdge, endEdge);
            }
        }
        

        public void MoveTo(Vector2 newPosition)
        {
            Position = newPosition;
        }

        private IEnumerator MoveToTarget(ICurve curve)
                {
                    isMoving = true;
                    lerpStartTime = Time.time;
                    Vector2 startPosition = this.Position;

                    while (Time.time - lerpStartTime < lerpDuration)
                    {
                        float t = (Time.time - lerpStartTime) / lerpDuration;
                        MoveTo(curve.Point(t));
                        yield return null;
                    }
                    MoveTo(curve.Point(1));
                    isMoving = false;
                    yield return null;

                }
        private IEnumerator EnterRoom(EdgeInfo edge)
        {
            List<Vector2> randomPoints = new List<Vector2>();
            randomPoints.Add(currEdge.Curve.Point(1));
            int points = UnityEngine.Random.Range(4, 10);
            
            for (int i = 0; i < 5; i++)  
            {
                Vector2 randomPoint = navGraph.GetVertex(edge.ToVertex).region.RandPoint(); 
                randomPoints.Add(randomPoint);
            }

            //changed post exit code
            var exit = path.Pop();
            randomPoints.Add(exit.Curve.Point(0));
            path = pathFinder.FindPath(exit, navGraph.GetRandomEdge());
            lerpStartTime = Time.time;
            Vector2 startPosition = this.Position;
            while (Time.time - lerpStartTime < lerpDurationRoom)
            {
                float t = (Time.time - lerpStartTime) / lerpDuration;
                Vector2 bezierPosition = DeCasteljauRecursive(randomPoints, t);
                MoveTo(bezierPosition);
                yield return null;
            }
                
            justEntered = true;
            isInRoom = false;
            yield return null;

        }

        private Vector2 DeCasteljauRecursive(List<Vector2> points, float t)
        {
            if (points.Count == 1)
            {
                return points[0];
            }

            List<Vector2> newPoints = new List<Vector2>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 newPoint = Vector2.Lerp(points[i], points[i + 1], t);
                newPoints.Add(newPoint);
            }

            return DeCasteljauRecursive(newPoints, t);
        }
    }
}



