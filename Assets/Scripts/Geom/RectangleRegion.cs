using BinaryPartition;
using UnityEngine;

namespace Geom
{
    public class RectangleRegion : IRegion
    {
        public Rectangle Rectangle;

        public RectangleRegion(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }

        public Vector2 CenterPoint => (Rectangle.Min + Rectangle.Max) / 2;
        public float Area
        {
            get
            {
                var dims = Rectangle.Max - Rectangle.Min;
                return dims.x * dims.y;
            }
        }

        public Vector2 RandPoint()
        {
            return new Vector2(
                Mathf.Lerp(Rectangle.Min.x, Rectangle.Max.x, Random.value),
                Mathf.Lerp(Rectangle.Min.y, Rectangle.Max.y, Random.value)
                );
        }
    }
}