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
        public List<List<EdgeInfo>> AdjList;
        public List<VertexInfo> Vertices;

        public Graph(List<List<EdgeInfo>> adjList, List<VertexInfo> vertices)
        {
            AdjList = adjList;
            Vertices = vertices;
        }

        public IEnumerable<ICurve> Curves()
        {
            foreach (var adj in AdjList)
            {
                foreach (var edge in adj)
                {
                    yield return edge.Curve;
                }
            }
        }

        public IEnumerable<Rectangle> Rectangles()
        {
            foreach (var vertex in Vertices)
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