using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            public int FromVertex { get; private set; }
            public bool Navigable { get;}

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

            public BEdge(int fromVertex, int toVertex, bool navigable, EdgeTag tag, ICurve curve)
            {
                FromVertex = fromVertex;
                Navigable = navigable;
                EdgeInfo = new EdgeInfo(toVertex, curve, tag);
            }

            private BEdge Clone()
            {
                return new BEdge(FromVertex, ToVertex, Navigable, Tag, Curve);
            }

            public (Vector2, BEdge) Split(float t, int midVertex)
            {
                var other = Clone();
                var (leftCurve, midPoint, rightCurve) = Curve.Split(t);

                Curve = leftCurve;
                ToVertex = midVertex;

                other.Curve = rightCurve;
                other.FromVertex = midVertex;

                return (midPoint, other);
            }
        }
        
        private readonly List<BEdge> _edges = new();
        private readonly List<VertexInfo> _vertexInfos = new();
        private int _vertCount = 0;
        private int _edgeCount = 0;

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

        public EdgeId MakeEdge(VertexId fromVertexId, VertexId toVertexId, bool navigable, EdgeTag tag, ICurve curve)
        {
            return AddEdge(new BEdge(fromVertexId.Id, toVertexId.Id, navigable, tag, curve));
        }
        
        public EdgeId MakeEdge(VertexId fromVertexId, VertexId toVertexId, bool navigable, EdgeTag tag)
        {
            var curve = new LineCurve(GetPosition(fromVertexId), GetPosition(toVertexId));
            return AddEdge(new BEdge(fromVertexId.Id, toVertexId.Id, navigable, tag, curve));
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

        public (EdgeId, VertexId, EdgeId) SplitEdge(EdgeId edgeId, float t, VertexTag vertexTag = VertexTag.None)
        {
            VertexInfo vertexInfo = new VertexInfo
            {
                tag = vertexTag
            };
            var vertexId = MakeVertex(vertexInfo);
            
            var (point, newEdge) = _edges[edgeId.Id].Split(t, vertexId.Id);
            var newEdgeId = AddEdge(newEdge);
            vertexInfo.region = new PointRegion(point);

            return (edgeId, vertexId, newEdgeId);
        }

        public Graph ToGraph()
        {
            List<List<EdgeInfo>> adjList = new();
            for (var i = 0; i < _vertCount; i++)
            {
                adjList.Add(new List<EdgeInfo>());
            }
            foreach (var edge in _edges.Where(edge => edge.Navigable))
            {
                adjList[edge.FromVertex].Add(edge.EdgeInfo);
            }

            return new Graph(adjList, _vertexInfos);
        }

    }
}