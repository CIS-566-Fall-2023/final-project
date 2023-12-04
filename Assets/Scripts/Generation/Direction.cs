using System;
using BinaryPartition;
using UnityEngine;

namespace Generation
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class Directions
    {
        public static Divider GetDivider(this Direction direction, DividerBounds[] bounds)
        {
            return direction switch
            {
                Direction.North => bounds[1].High,
                Direction.East => bounds[0].High,
                Direction.South => bounds[1].Low,
                Direction.West => bounds[0].Low,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static (Vector2, Vector2) GetRectangleSide(this Direction direction, Rectangle rectangle)
        {
            return direction switch
            {
                Direction.North => (
                    new Vector2(rectangle.Min.x, rectangle.Max.y),
                    new Vector2(rectangle.Max.x, rectangle.Max.y)
                    ),
                Direction.East => (
                    new Vector2(rectangle.Max.x, rectangle.Max.y),
                    new Vector2(rectangle.Max.x, rectangle.Min.y)
                ),
                Direction.South => (
                    new Vector2(rectangle.Max.x, rectangle.Min.y),
                    new Vector2(rectangle.Min.x, rectangle.Min.y)
                ),
                Direction.West => (
                    new Vector2(rectangle.Min.x, rectangle.Min.y),
                    new Vector2(rectangle.Min.x, rectangle.Max.y)
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        public static Vector2 ToVector(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Vector2.up,
                Direction.East =>  Vector2.right,
                Direction.South => Vector2.down,
                Direction.West => Vector2.left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction Perp(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}