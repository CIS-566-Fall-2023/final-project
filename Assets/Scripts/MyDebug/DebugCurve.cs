using Geom;
using UnityEngine;

namespace MyDebug
{
    public class DebugCurve : IDebugDrawable
    {
        public ICurve Curve;
        public Color Color;
        
        public void Draw()
        {
            foreach (var (a, b) in Curve.ToPointStream().Pairwise())
            {
                var segment = new DebugSegment { P0 = a, P1 = b, Color = Color };
                segment.Draw();
            }
        }
    }
}