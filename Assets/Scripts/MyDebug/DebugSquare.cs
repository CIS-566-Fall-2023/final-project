using BinaryPartition;
using UnityEngine;
using GraphBuilder;

namespace MyDebug
{
    public class DebugSquare : IDebugDrawable
    {
        public Rectangle Rectangle;
        public Color Color;
        public Vector2 position;
        public VertexId CurrentVertex { get; set; }

        public DebugSquare()
        {
            Rectangle = Rectangle.UnitSquare();
            Color = Color.red;
            position = new Vector2(0,0);
        }

        public void MoveTo(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw()
        {
            Debug.DrawLine(new Vector3(Rectangle.Min[0] + position.x, 0, Rectangle.Min[1] + position.y), new Vector3(Rectangle.Min[0] + position.x, 0, Rectangle.Max[1] + position.y), Color);
            Debug.DrawLine(new Vector3(Rectangle.Min[0] + position.x, 0, Rectangle.Max[1] + position.y), new Vector3(Rectangle.Max[0] + position.x, 0, Rectangle.Max[1] + position.y), Color);
            Debug.DrawLine(new Vector3(Rectangle.Max[0] + position.x, 0, Rectangle.Max[1] + position.y), new Vector3(Rectangle.Max[0] + position.x, 0, Rectangle.Min[1] + position.y), Color);
            Debug.DrawLine(new Vector3(Rectangle.Max[0] + position.x, 0, Rectangle.Min[1] + position.y), new Vector3(Rectangle.Min[0] + position.x, 0, Rectangle.Min[1] + position.y), Color);
        }
    }
}