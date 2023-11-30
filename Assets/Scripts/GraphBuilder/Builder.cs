using System;
using System.Collections.Generic;
using Geom;
using Navigation;
using UnityEngine;

namespace GraphBuilder
{
    public class Builder
    {
        public struct BEdge
        {
            public EdgeInfo EdgeInfo;
            public int FromVertex { get; }
            public int ToVertex 
            { 
                get => EdgeInfo.ToVertex;
                private set => EdgeInfo.ToVertex = value; 
            }

            public ICurve Curve { 
                get => EdgeInfo.Curve;
                private set => EdgeInfo.Curve = value; 
            }
            public EdgeTag Tag { 
                get => EdgeInfo.Tag;
                private set => EdgeInfo.Tag = value; 
            }

            public BEdge(int fromVertex, int toVertex, EdgeTag tag, ICurve curve)
            {
                FromVertex = fromVertex;
                EdgeInfo = new EdgeInfo(toVertex, curve, tag);
            }

            public BEdge Reverse()
            {
                return new BEdge(ToVertex, FromVertex, Tag, Curve.Reverse());
            }
        }
        
        private readonly List<BEdge> _edges = new();
        private readonly List<VertexInfo> _vertexInfos = new();
        private int _vertCount = 0;
        private int _edgeCount = 0;

        public Builder()
        {
        }

        public VertexId MakeVertex(Vector2 point, VertexTag vertexTag = VertexTag.None)
        {
            return MakeVertex(new VertexInfo(new PointRegion(point), vertexTag));
        }
        public VertexId MakeVertex(VertexInfo vertexInfo)
        {
            _vertexInfos.Add(vertexInfo);
            return new VertexId(_vertCount++);
        }

        public Vector2 GetPosition(VertexId vertexId)
        {
            return _vertexInfos[vertexId.Id].region.CenterPoint;
        }

        private EdgeId AddEdge(BEdge edge)
        {
            _edges.Add(edge);
            return new EdgeId(_edgeCount++);
        }
        
        public EdgeId MakeEdge(VertexId fromVertexId, VertexId toVertexId, EdgeTag tag, ICurve curve)
        {
            return AddEdge(new BEdge(fromVertexId.Id, toVertexId.Id, tag, curve));
        }
        
        public EdgeId MakeEdge(VertexId fromVertexId, VertexId toVertexId, EdgeTag tag)
        {
            var curve = new LineCurve(GetPosition(fromVertexId), GetPosition(toVertexId));
            return AddEdge(new BEdge(fromVertexId.Id, toVertexId.Id, tag, curve));
        }

        public VertexId GetFromVertex(EdgeId edgeId)
        {
            return new VertexId(_edges[edgeId.Id].FromVertex);
        }
        
        public VertexId GetToVertex(EdgeId edgeId)
        {
            return new VertexId(_edges[edgeId.Id].ToVertex);
        }
        
        public ICurve GetCurve(EdgeId edgeId)
        {
            return _edges[edgeId.Id].Curve;
        }
        
        public Graph ToGraph()
        {
            List<List<EdgeInfo>> adjList = new();
            for (var i = 0; i < _vertCount; i++)
            {
                adjList.Add(new List<EdgeInfo>());
            }
            foreach (var edge in _edges)
            {
                adjList[edge.FromVertex].Add(edge.EdgeInfo);
                var reversed = edge.Reverse();
                adjList[reversed.FromVertex].Add(reversed.EdgeInfo);
            }

            return new Graph(adjList, _vertexInfos);
        }

    }
}