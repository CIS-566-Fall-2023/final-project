using System.Collections.Generic;
using BinaryPartition;
using MyDebug;
using UnityEngine;

namespace Navigation {
    public class PathFinder {    
        private Graph navGraph;

        public PathFinder(Graph navGraph)
            {
            this.navGraph = navGraph;
            }

        public Stack<EdgeInfo> FindPath(EdgeInfo start, EdgeInfo end)
{
    Dictionary<EdgeInfo, bool> visited = new Dictionary<EdgeInfo, bool>();
    foreach (List<EdgeInfo> edgeList in navGraph.GetAdjList())
    {
        foreach (EdgeInfo edge in edgeList)
        {
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
        if (edge.Curve == end.Curve)
        {
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
        return new Stack<EdgeInfo>();

    Stack<EdgeInfo> result = new Stack<EdgeInfo>();
    EdgeInfo current = end;
    while (current.Curve != start.Curve)
    {
        result.Push(current);
        current = path[current];
    }
    result.Push(start);

    return result;
}



    }
}


