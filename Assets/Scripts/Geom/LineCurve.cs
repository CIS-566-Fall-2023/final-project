using System.Collections.Generic;
using UnityEngine;

namespace Geom
{
    public class LineCurve : ICurve
    {
        public Vector2 P0;
        public Vector2 P1;

        public LineCurve(Vector2 p0, Vector2 p1)
        {
            P0 = p0;
            P1 = p1;
        }

        public Vector2 AsVector()
        {
            return P1 - P0;
        }

        public float Length()
        {
            return AsVector().magnitude;
        }

        public Vector2 Point(float t)
        {
            return Vector2.Lerp(P0, P1, t);
        }

        public float TangentAngle(float t)
        {
            Vector2 vec = AsVector();
            return Mathf.Atan2(vec.y, vec.x);
        }

        public (ICurve, Vector2, ICurve) Split(float t)
        {
            var point = Point(t);
            return (new LineCurve(P0, point), point, new LineCurve(point, P1));
        }

        public ICurve Reverse()
        {
            return new LineCurve(P1, P0);
        }
    }
}