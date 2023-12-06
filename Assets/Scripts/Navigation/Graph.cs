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
                if (vertex.region is RectangleRegion)
                {
                    yield return ((RectangleRegion)vertex.region).Rectangle;
                }
            }
        }

        public VertexInfo GetVertex(int vertexId)
        {
            return _vertices[vertexId];
        }

        public List<List<EdgeInfo>> GetAdjList()
        {
            return _adjList;
        }

        public VertexInfo GetRandomVertex()
            {
                int index = Random.Range(0, _vertices.Count-1);
                return _vertices[index];
            }

        public EdgeInfo GetRandomEdge()
            {
                int index = Random.Range(0, _adjList.Count-1);
                if (_adjList[index].Count > 0)
                    return _adjList[index][0];
                else 
                    return GetRandomEdge();
            }

        public List<EdgeInfo> GetAdjacentEdges(EdgeInfo edge)
            {
                return _adjList[edge.ToVertex];
            }


        // TODO: Insha implements pathfinding algos
    }
}