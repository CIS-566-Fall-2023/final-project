using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Navigation
{
    public class Graph
    {
        private List<List<EdgeInfo>> _adjList;
        private List<VertexInfo> _vertices;

        public Graph(List<List<EdgeInfo>> adjList, List<VertexInfo> vertices)
        {
            _adjList = adjList;
            _vertices = vertices;
        }

        // TODO: Insha implements pathfinding algos
    }
}