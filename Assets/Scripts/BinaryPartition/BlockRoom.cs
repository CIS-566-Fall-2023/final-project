using Geom;
using GraphBuilder;
using Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BinaryPartition
{
    public class BlockRoom
    {
        private static readonly Vector2 Trim = new(1, 1);
        private const float DoorMargin = 4;

        private readonly DividerBounds[] _dividers;
        private Rectangle _bigRect;
        private Rectangle _smallRect;
        private readonly Builder _builder;
        private readonly VertexId _roomVert;

        public BlockRoom(DividerBounds[] dividers, Rectangle rectangle, Builder builder)
        {
            _dividers = dividers;
            _bigRect = rectangle;
            _smallRect = new Rectangle
            {
                Min = _bigRect.Min + Trim,
                Max = _bigRect.Max - Trim,
            };
            _builder = builder;
            _roomVert = builder.MakeVertex(new VertexInfo(new RectangleRegion(_smallRect), VertexTag.Room));
        }

        public void AddDoorways()
        {
            foreach (var parAxis in new[] { 0, 1 })
            {
                foreach (var low in new[] { true, false })
                {
                    if (Random.value <= 0.5)
                    {
                        AddDoorway(parAxis, low);
                    }
                }
            }
        }

        private void AddDoorway(int parAxis, bool low)
        {
            var divider = low ? _dividers[parAxis].Low : _dividers[parAxis].High;
            if (divider == null)
            {
                return;
            }
            var perpAxis = 1 - parAxis;
            var val = Mathf.Lerp(
                _smallRect.Min[perpAxis] + DoorMargin,
                _smallRect.Max[perpAxis] - DoorMargin,
                Random.value);
            var position = new Vector2
            {
                [parAxis] = low ? _bigRect.Min[parAxis] : _bigRect.Max[parAxis],
                [perpAxis] = val
            };
            var vert = _builder.MakeVertex(position);
            _builder.MakeEdge(vert, _roomVert, EdgeTag.Doorway, new LineCurve(position, new Vector2
            {
                [parAxis] = low ? _smallRect.Min[parAxis] : _bigRect.Max[parAxis],
                [perpAxis] = val
            }));

            if (low)
            {
                divider.AddBelow(vert);
            }
            else
            {
                divider.AddAbove(vert);
            }
        }
    }
}