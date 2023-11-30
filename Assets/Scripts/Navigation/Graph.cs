using System.Collections;
using System.Collections.Generic;
using BinaryPartition;
using Geom;
using UnityEditor.Experimental.GraphView;
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

        public IEnumerable<ICurve> Curves()
        {
            foreach (var adj in _adjList)
            {
                foreach (var edge in adj)
                {
                    yield return edge.Curve;
                }
            }
        }

        public IEnumerable<Rectangle> Rectangles()
        {
            foreach (var vertex in _vertices)
            {
                if (vertex.region is RectangleRegion)
                {
                    yield return ((RectangleRegion)vertex.region).Rectangle;
                }
            }
        }

        // TODO: Insha implements pathfinding algos
    }
}