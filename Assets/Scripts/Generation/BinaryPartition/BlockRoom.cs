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
        private const float WallThickness = 4;
        private static readonly Vector2 WallVec = new Vector2(WallThickness, WallThickness);

        private readonly DividerBounds[] _dividers;
        private Rectangle _hallRect;
        private Rectangle _innerRect;
        private Rectangle _outerRect;
        private readonly BuildingGenerator _generator;
        private readonly VertexId _roomVert;

        private Builder Builder => _generator.Builder;

        public BlockRoom(DividerBounds[] dividers, Rectangle rectangle, BuildingGenerator generator)
        {
            _dividers = dividers;
            _hallRect = rectangle;
            _outerRect = new Rectangle
            {
                Min = _hallRect.Min + TrimVec,
                Max = _hallRect.Max - TrimVec,
            };
            _innerRect = new Rectangle
            {
                Min = _outerRect.Min + WallVec,
                Max = _outerRect.Max - WallVec,
            };
            _generator = generator;
            _roomVert = generator.Builder.MakeVertex(new VertexInfo(new RectangleRegion(_innerRect), VertexTag.Room));
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
            var (lowInner, highInner) = direction.GetRectangleSide(_innerRect);
            var (lowOuter, highOuter) = direction.GetRectangleSide(_outerRect);

            if (divider == null || Random.value < 0.2)
            {
                _generator.AddWall(new LineCurve(lowInner, highInner));
                _generator.AddWall(new LineCurve(lowOuter, highOuter));
                return;
            }
            
            var perp = direction.Perp().ToVector();
            
            var doorInnerPosition = Vector2.Lerp(
                lowInner + perp * (WallThickness + DoorMargin),
                highInner - perp * (WallThickness + DoorMargin),
                Random.value);

            var lowInnerDoorPosition = doorInnerPosition - perp * DoorSize;
            var highInnerDoorPosition = doorInnerPosition + perp * DoorSize;

            var doorOuterPosition = doorInnerPosition + direction.ToVector() * WallThickness;

            var lowOuterDoorPosition = doorOuterPosition - perp * DoorSize;
            var highOuterDoorPosition = doorOuterPosition + perp * DoorSize;
            
            _generator.AddWall(new LineCurve(lowInner, lowInnerDoorPosition));
            _generator.AddWall(new LineCurve(highInner, highInnerDoorPosition));
            
            _generator.AddWall(new LineCurve(lowOuter, lowOuterDoorPosition));
            _generator.AddWall(new LineCurve(highOuter, highOuterDoorPosition));
            
            _generator.AddWall(new LineCurve(lowOuterDoorPosition, lowInnerDoorPosition));
            _generator.AddWall(new LineCurve(highInnerDoorPosition, highOuterDoorPosition));

            var hallPosition = doorInnerPosition + direction.ToVector() * (Trim + WallThickness);
            var hallVert = Builder.MakeVertex(hallPosition);
            
            Builder.MakeEdge(hallVert, _roomVert, EdgeTag.Doorway,
                new LineCurve(
                    hallPosition,
                    doorInnerPosition
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