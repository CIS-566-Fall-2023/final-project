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