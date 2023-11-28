using BinaryPartition;
using UnityEngine;
namespace MyDebug
{
    public class DebugRect : IDebugDrawable
    {
        public Rectangle Rectangle;
        public Color Color;

        public DebugRect()
        {
            Rectangle = Rectangle.UnitSquare();
            Color = Color.blue;
        }

        public void Draw()
        {
            Debug.DrawLine(new Vector3(Rectangle.Min[0],0,  Rectangle.Min[1]), new Vector3(Rectangle.Min[0], 0, Rectangle.Max[1]), Color);
            Debug.DrawLine(new Vector3(Rectangle.Min[0], 0, Rectangle.Max[1]), new Vector3(Rectangle.Max[0], 0, Rectangle.Max[1]), Color);
            Debug.DrawLine(new Vector3(Rectangle.Max[0], 0, Rectangle.Max[1]), new Vector3(Rectangle.Max[0], 0, Rectangle.Min[1]), Color);
            Debug.DrawLine(new Vector3(Rectangle.Max[0], 0, Rectangle.Min[1]), new Vector3(Rectangle.Min[0], 0, Rectangle.Min[1]), Color);
        }
        
    }
}