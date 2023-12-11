using System.Collections;
using System.Collections.Generic;
using BinaryPartition;
using Geom;
using UnityEngine;

namespace Navigation
{
    public class Graph
    {
        public List<List<EdgeInfo>> _adjList;
        public List<VertexInfo> _vertices;

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
                if (vertex.region is RectangleRegion region)
                {
                    yield return region.Rectangle;
                }
            }
        }

        public VertexInfo GetVertex(int vertexId)
        {
            return _vertices[vertexId];
        }

        public EdgeInfo GetRandomEdge()
        {
            while (true)
            {
                var index = Random.Range(0, _adjList.Count - 1);
                if (_adjList[index].Count > 0) return _adjList[index][0];
            }
        }

        public EdgeInfo GetNextEdge(int vertex)
        {
            var edges = _adjList[vertex];
            return edges[Random.Range(0, edges.Count)];
        }

        // public List<EdgeInfo> GetAdjacentEdges(EdgeInfo edge)
        // {
        //     return _adjList[edge.ToVertex];
        // }
    }
}