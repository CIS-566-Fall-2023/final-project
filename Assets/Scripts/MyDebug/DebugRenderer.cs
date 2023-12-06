using System.Collections.Generic;
using System.Linq;
using Generation;
using Geom;
using GraphBuilder;
using Navigation;
using UnityEngine;

namespace MyDebug
{
    public class DebugRenderer : MonoBehaviour
    {
        private List<IDebugDrawable> _drawables = new();
        private DebugSquare sprite = new DebugSquare();
        private int tick = 0;
        private Graph navGraph;
        private EdgeInfo currEdge; 
        private Builder builder = new Builder();
        
        /*** DFS stuff ***/ 
        private EdgeInfo startEdge;
        private EdgeInfo endEdge;
        private List<EdgeInfo> path;

        /*** Move stuff ***/ 
        private bool isMoving = false;
        private Vector2 targetPosition;
        private float lerpDuration = 1.0f; // You can adjust the duration to control the speed of movement
        private float lerpStartTime;
        private BezierCurveMover curveMover;

        private List<ICurve> _walls;
        
        private int _vertexId = 0;
        private int _edgeId = 0;
        private int _incidentEdgeCount = 0;
  
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Running Debug Renderer");
            BuildingGenerator generator = new BuildingGenerator();
            generator.GenerateBuilding();
            _walls = generator.GetWalls().ToList();
            navGraph = generator.Builder.ToGraph();

            foreach (var curve in navGraph.Curves())
            {
                _drawables.Add(new DebugCurve(curve, Color.green));
            }
            
            foreach (var curve in _walls)
            {
                _drawables.Add(new DebugCurve(curve, Color.blue));
            }
            
            startEdge = navGraph.GetRandomEdge();
            endEdge = navGraph.GetRandomEdge();
            currEdge = navGraph.GetRandomEdge();
            sprite.MoveTo(currEdge.Curve.Point(0));
            path = DFS(startEdge, endEdge);
        }

        List<EdgeInfo> DFS(EdgeInfo start, EdgeInfo end) {
            Dictionary<EdgeInfo, bool> visited = new ();
            foreach (var edge in navGraph.GetAdjList().SelectMany(edgeList => edgeList))
            {
                visited[edge] = false;
            }
            Stack<EdgeInfo> stack = new();
            Dictionary<EdgeInfo, EdgeInfo> parent = new ();
            stack.Push(start);
            visited[start] = true;
            while (stack.Count > 0)
            {
                EdgeInfo edge = stack.Pop();
                if (edge.Curve == end.Curve) {
                     break;
                }
                foreach (var adjEdge in navGraph.GetAdjacentEdges(edge)
                             .Where(adjEdge => !visited.ContainsKey(adjEdge) || !visited[adjEdge]))
                {
                    visited[adjEdge] = true;
                    stack.Push(adjEdge);
                    parent[adjEdge] = edge;
                }
               
            }

            if (!parent.ContainsKey(end))
            {
                return new List<EdgeInfo>();
            }
                
            List<EdgeInfo> result = new ();
            var current = end;
            while (current.Curve != start.Curve)
            {
                result.Add(current);
                current = parent[current];
            }
            result.Add(start); 
            result.Reverse(); 

            return result;
        }

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

        private IEnumerator<object> EnterRoom(EdgeInfo edge){
            curveMover = new BezierCurveMover(this, 5.0f); 
            List<Vector2> randomPoints = new List<Vector2>();
            randomPoints.Add(((LineCurve)currEdge.Curve).P0);
            int points = UnityEngine.Random.Range(1, 10);
            for (int i = 0; i < 5; i++)  
            {
                Vector2 randomPoint = navGraph.GetVertices()[edge.ToVertex].region.RandPoint(); 
                randomPoints.Add(randomPoint);
            }
            if (path[0].Tag == EdgeTag.Doorway) {
                randomPoints.Add(((LineCurve)path[0].Curve).P0);
            }
            else {
                path = DFS(edge, endEdge);
                randomPoints.Add(((LineCurve)currEdge.Curve).P0); 
            }
            curveMover.SetControlPoints(randomPoints);
            curveMover.MoveSpriteAlongCurve(sprite);
            return null;
        }


        //renderer
        // Update is called once per frame
        private void Update()
        {
            foreach (var drawable in _drawables)
            {
                drawable.Draw();
            }
            tick++;
            sprite.Draw();
            if (tick % 60 != 0 || isMoving) return;
            if (path.Count > 0)
            {
                // Get the next edge in the path
                currEdge = path[0];
                path.RemoveAt(0);
                StartCoroutine(MoveToTarget(currEdge.Curve));
                    
                /*if (currEdge.Tag == EdgeTag.Doorway)
                    {
                        //targetPosition = ((LineCurve)currEdge.Curve).P0;
                        StartCoroutine(MoveToTarget(currEdge.Curve));
                        StartCoroutine(EnterRoom(currEdge));
                    }
                    else
                    {
                        //targetPosition = ((LineCurve)currEdge.Curve).P0;
                        StartCoroutine(MoveToTarget(currEdge.Curve));
                    }
                    */
            }
            else
            {
                endEdge = navGraph.GetRandomEdge();
                path = DFS(currEdge, endEdge);
            }
        }
    }
}


