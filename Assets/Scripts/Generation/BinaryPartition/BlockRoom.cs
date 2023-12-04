using System;
using Generation;
using Geom;
using GraphBuilder;
using Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BinaryPartition
{
    public class BlockRoom
    {
        private const float Trim = 2;
        private static readonly Vector2 TrimVec = new Vector2(Trim, Trim);
        private const float DoorMargin = 4;
        private const float DoorSize = 2;

        private readonly DividerBounds[] _dividers;
        private Rectangle _bigRect;
        private Rectangle _smallRect;
        private readonly BuildingGenerator _generator;
        private readonly VertexId _roomVert;

        private Builder Builder => _generator.Builder;

        public BlockRoom(DividerBounds[] dividers, Rectangle rectangle, BuildingGenerator generator)
        {
            _dividers = dividers;
            _bigRect = rectangle;
            _smallRect = new Rectangle
            {
                Min = _bigRect.Min + TrimVec,
                Max = _bigRect.Max - TrimVec,
            };
            _generator = generator;
            _roomVert = generator.Builder.MakeVertex(new VertexInfo(new RectangleRegion(_smallRect), VertexTag.Room));
        }

        public void MakeWallsAndDoors()
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                MakeWall(direction);
            }
        }

        private void MakeWall(Direction direction)
        {
            var divider = direction.GetDivider(_dividers);
            var (low, high) = direction.GetRectangleSide(_smallRect);

            if (divider == null || Random.value < 0.2)
            {
                _generator.AddWall(new LineCurve(low, high));
                return;
            }
            
            var perp = direction.Perp().ToVector();
            var doorPosition = Vector2.Lerp(
                low + perp * DoorMargin,
                high - perp * DoorMargin,
                Random.value);
            
            _generator.AddWall(new LineCurve(low, doorPosition - perp * DoorSize));
            _generator.AddWall(new LineCurve(high, doorPosition + perp * DoorSize));

            var hallPosition = doorPosition + direction.ToVector() * Trim;
            var hallVert = Builder.MakeVertex(hallPosition);
            
            Builder.MakeEdge(hallVert, _roomVert, EdgeTag.Doorway,
                new LineCurve(
                    hallPosition,
                    doorPosition
            ));

            switch (direction)
            {
                case Direction.North:
                case Direction.East:
                {
                    divider.AddAbove(hallVert);
                    break;
                }
                case Direction.South:
                case Direction.West:
                {
                    divider.AddBelow(hallVert);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}