using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Geom;
using UnityEngine;

#nullable enable

namespace Navigation
{
    public static class PathFinder
    {
        class PathStep
        {
            public PathStep? Parent;
            public ICurve Curve;

            public PathStep(PathStep? parent, ICurve curve)
            {
                Parent = parent;
                Curve = curve;
            }
        }

        public static (List<ICurve>, int) RandomPath(Graph graph, int source)
        {
            while (true)
            {
                var target = Random.Range(0, graph.NumVerts);
                var path = FindPath(graph, source, target);
                if (path.Count > 0) return (path, target);
            }
        }

        public static List<ICurve> FindPath(Graph graph, int source, int target)
        {
            var seen = new BitArray(graph.NumVerts);
            var pathEndingIn = new PathStep[graph.NumVerts];
            Stack<int> stack = new();
            stack.Push(source);
            while (stack.Count > 0)
            {
                int u = stack.Pop();
                if (u == target)
                {
                    break;
                }
                foreach (var edge in graph.GetAdj(u))
                {
                    int v = edge.ToVertex;
                    if (seen[v]) continue;
                    seen[v] = true;
                    stack.Push(v);
                    var endingStep = new PathStep(pathEndingIn[u], edge.Curve);
                    // if (graph.GetVertex(v).tag == VertexTag.Room)
                    // {
                    //     var roomCurve = GenerateRoomCurve(
                    //         pathEndingIn[u].Curve.Point(1),
                    //         endingStep.Curve.Point(0));
                    //     var roomStep = new PathStep(pathEndingIn[u], roomCurve);
                    //     endingStep.Parent = roomStep;
                    // }
                    pathEndingIn[v] = endingStep;
                }
            }
            
            List<ICurve> edges = new();
            var backStep = pathEndingIn[target];
            while (backStep != null)
            {
                edges.Add(backStep.Curve);
                backStep = backStep.Parent;
            }

            edges.Reverse();
            return edges;
        }
        
        static ICurve GenerateRoomCurve(Vector2 entrance, Vector2 exit)
        {
            return new LineCurve(entrance, exit);
        }
    }
}

// using System.Collections.Generic;
// using System.Linq;
//
// namespace Navigation
// {
//     public class PathFinder
//     {
//         private readonly Graph _navGraph;
//
//         public PathFinder(Graph navGraph)
//         {
//             _navGraph = navGraph;
//         }
//
//         public Stack<EdgeInfo> FindPath(EdgeInfo start, EdgeInfo end)
//         {
//             var visited = new Dictionary<EdgeInfo, bool>();
//             foreach (var edge in _navGraph.GetAdjList().SelectMany(edgeList => edgeList))
//             {
//                 visited[edge] = false;
//             }
//
//             var stack = new Stack<EdgeInfo>();
//             var path = new Dictionary<EdgeInfo, EdgeInfo>();
//             stack.Push(start);
//             visited[start] = true;
//
//             while (stack.Count > 0)
//             {
//                 EdgeInfo edge = stack.Pop();
//                 if (edge.Curve == end.Curve)
//                 {
//                     break;
//                 }
//
//                 foreach (var adjEdge in _navGraph.GetAdjacentEdges(edge).Where(adjEdge => !visited.ContainsKey(adjEdge) || !visited[adjEdge]))
//                 {
//                     visited[adjEdge] = true;
//                     stack.Push(adjEdge);
//                     path[adjEdge] = edge;
//                 }
//             }
//
//             if (!path.ContainsKey(end))
//                 return new Stack<EdgeInfo>();
//
//             var result = new Stack<EdgeInfo>();
//             var current = end;
//             while (current.Curve != start.Curve)
//             {
//                 result.Push(current);
//                 current = path[current];
//             }
//
//             result.Push(start);
//
//             return result;
//         }
//     }
// }