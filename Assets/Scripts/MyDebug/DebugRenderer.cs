using System;
using System.Collections.Generic;
using BinaryPartition;
using Generation;
using Geom;
using GraphBuilder;
using Navigation;
using UnityEditor;
using UnityEngine;

namespace MyDebug
{
    public class DebugRenderer : MonoBehaviour
    {
        private List<IDebugDrawable> _drawables = new();
        private List<Wanderer> feet = new List<Wanderer>();
        private WandererManager wandererManager;
        //private int tick = 0;
        private Graph navGraph;
        private EdgeInfo currEdge; 
        private Builder builder = new Builder();

        public Sprite sprite;

        /*** DFS stuff 
        private EdgeInfo startEdge;
        private EdgeInfo endEdge;
        private List<EdgeInfo> path;
        ***/

        /*** Move stuff ***/
        //private float lerpDuration = 1.0f; // You can adjust the duration to control the speed of movement
        private float lerpStartTime;
        /*
        private bool justEntered = false;
        */
        
  
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Running Debug Renderer");

            BuildingGenerator generator = new();
            generator.GenerateBuilding();
            navGraph = generator.Builder.ToGraph();
            wandererManager = new GameObject("Wanderer Manager").AddComponent<WandererManager>();
            wandererManager.Initialize(navGraph);

            foreach (var curve in navGraph.Curves())
            {
                var lineCurve = curve;
                _drawables.Add(new DebugCurve(curve, Color.green));
            }
    
            foreach (var rectangle in navGraph.Rectangles())
            {
                _drawables.Add(new DebugRect {Rectangle = rectangle, Color = Color.blue});
            }

}

/*
        List<EdgeInfo> DFS(EdgeInfo start, EdgeInfo end) {
                Dictionary<EdgeInfo, bool> visited = new Dictionary<EdgeInfo, bool>();
                foreach(List<Navigation.EdgeInfo> edgeList in navGraph.GetAdjList()) {
                    foreach(EdgeInfo edge in edgeList) {
                        visited[edge] = false;
                    }
                }
                Stack<EdgeInfo> stack = new Stack<EdgeInfo>();
                Dictionary<EdgeInfo, EdgeInfo> path = new Dictionary<EdgeInfo, EdgeInfo>();
                stack.Push(start);
                visited[start] = true;
                while (stack.Count > 0)
                    {
                        EdgeInfo edge = stack.Pop();
                        if (edge.Curve == end.Curve) {
                             break;
                        }
                        foreach (EdgeInfo adjEdge in navGraph.GetAdjacentEdges(edge))
                        {
                            if (!visited.ContainsKey(adjEdge) || !visited[adjEdge])
                                {
                                    visited[adjEdge] = true;
                                    stack.Push(adjEdge);
                                    path[adjEdge] = edge;  
                                }
                        }
                       
                    }
                if (!path.ContainsKey(end))
                    return new List<EdgeInfo>();
                List<EdgeInfo> result = new List<EdgeInfo>();
                EdgeInfo current = end;
                while (current.Curve != start.Curve)
                {
                    result.Add(current);
                    current = path[current];
                }
                result.Add(start); 
                result.Reverse(); 

                return result;

            }
            */

            /*

            private IEnumerator<object> MoveToTarget(ICurve curve)
                {
                    isMoving = true;
                    lerpStartTime = Time.time;
                    Vector2 startPosition = sprite.position;

                    while (Time.time - lerpStartTime < lerpDuration)
                    {
                        float t = (Time.time - lerpStartTime) / lerpDuration;
                        sprite.MoveTo(curve.Point(t));
                       // sprite.MoveTo(Vector2.Lerp(startPosition, target, t));
                        yield return null;
                    }

                    sprite.MoveTo(curve.Point(1));
                    isMoving = false;
                }
                */

/*
            private IEnumerator<object> EnterRoom(EdgeInfo edge){
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
                    path = DFS(exit, navGraph.GetRandomEdge());
                    curveMover.SetControlPoints(randomPoints);
                    curveMover.MoveSpriteAlongCurve(sprite);
                    justEntered = true;
                    return null;
            }

*/

        //renderer
        // Update is called once per frame
        void Update()
            {
                foreach (var drawable in _drawables)
                {
                    drawable.Draw();
                }


            /*
                tick++;
                sprite.Draw();

                
                if (tick % 60 == 0)
                {
                    if (!isMoving && path.Count > 0)
                    {
                        // Get the next edge in the path
                        currEdge = path[0];
                        path.RemoveAt(0);
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
                    else if (!isMoving)
                    {
                        endEdge = navGraph.GetRandomEdge();
                        path = DFS(currEdge, endEdge);
                    }
                }
                */
            }


        }
}