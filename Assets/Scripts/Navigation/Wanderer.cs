using BinaryPartition;
using Geom;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Navigation {

    public class Wanderer : MonoBehaviour {
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public bool isMoving = false;
        private PathFinder pathFinder;
        public Graph navGraph;
        public Stack<EdgeInfo> path;
        public EdgeInfo startEdge { get; set; }
        public EdgeInfo currEdge { get; set; }
        public EdgeInfo endEdge { get; set; }
        public bool justEntered = false;

        // MOVEMENT
        private float lerpDuration = 1.0f; // You can adjust the duration to control the speed of movement
        private float lerpStartTime;

        // yum BEZIER STUFF
        private BezierCurveMover curveMover;

        public void Initialize(Graph navGraph, EdgeInfo start, EdgeInfo end, PathFinder pathFinder) {

            this.navGraph = navGraph;
            this.Position = start.Curve.Point(0);
            this.startEdge = start;
            this.endEdge = end;
            this.pathFinder = pathFinder;
            this.path = pathFinder.FindPath(start, end);
        }


        void Update(){
            if (!isMoving  && path.Count > 0) {
                currEdge = path.Pop();
                StartCoroutine(MoveToTarget(currEdge.Curve));
                if (currEdge.Tag == EdgeTag.Doorway && justEntered == false)
                        {
                            //targetPosition = ((LineCurve)currEdge.Curve).P0;
                            StartCoroutine(MoveToTarget(currEdge.Curve));
                            StartCoroutine(EnterRoom(currEdge));
                            justEntered = true;
                           
                        }
                        else
                        {
                            //targetPosition = ((LineCurve)currEdge.Curve).P0;
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

        private IEnumerator<object> MoveToTarget(ICurve curve)
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
        private IEnumerator<object> EnterRoom(EdgeInfo edge)
                {
                    curveMover = new BezierCurveMover(this, 5.0f); 
                    List<Vector2> randomPoints = new List<Vector2>();
                    randomPoints.Add(((LineCurve)currEdge.Curve).P0);
                    int points = UnityEngine.Random.Range(4, 10);
                    
                    for (int i = 0; i < 5; i++)  
                        {
                            Vector2 randomPoint = navGraph.GetVertices()[edge.ToVertex].region.RandPoint(); 
                            randomPoints.Add(randomPoint);
                        }

                    //changed post exit code
                    var exit = navGraph.GetAdjList()[edge.ToVertex][0];
                    randomPoints.Add(((LineCurve)exit.Curve).P0);
                    path = pathFinder.FindPath(exit, navGraph.GetRandomEdge());
                    curveMover.SetControlPoints(randomPoints);
                    curveMover.MoveSpriteAlongCurve(this);
                    justEntered = true;
                    return null;
                }

        
    }
  




}



