using UnityEngine;

namespace MyDebug
{
    public struct DebugSegment : IDebugDrawable
    {
        public Vector2 P0;
        public Vector2 P1;
        public Color Color;

        public void Draw()
        {
            Debug.DrawLine(new Vector3(P0.x, 0, P0.y), new Vector3(P1.x, 0, P1.y), Color);
            
            //Vector3 perp = new Vector3(P0.y, 0, -P1.x);
            //Gizmos.DrawSphere(new Vector3(P0.x, 0, P0.y), 1);
            //Gizmos.DrawSphere(new Vector3(P1.x, 0, P1.y), 1);
        }
    }
}