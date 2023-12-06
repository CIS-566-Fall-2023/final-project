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

        public Vector2 Tangent(float t = 0)
        {
            return P1 - P0;
        }

        public float Length()
        {
            return Tangent().magnitude;
        }

        public Vector2 Point(float t)
        {
            return Vector2.Lerp(P0, P1, t);
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