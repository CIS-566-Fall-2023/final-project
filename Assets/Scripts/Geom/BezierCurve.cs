using System.Linq;
using UnityEngine;

namespace Geom
{
    public class BezierCurve : ICurve
    {
        public readonly Vector2[] Points;
        private const int Segments = 10;

        public BezierCurve(Vector2[] points)
        {
            Points = points;
        }

        public float Length()
        {
            float length = 0f;
            Vector2 prevPoint = Point(0);

            for (int i = 1; i <= Segments; i++)
            {
                float t = i / (float)Segments;
                Vector2 currentPoint = Point(t);
                length += Vector2.Distance(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }

            return length;
        }

        public Vector2 Point(float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 p = uuu * Points[0];
            p += 3 * uu * t * Points[1];
            p += 3 * u * tt * Points[2];
            p += ttt * Points[3];

            return p;
        }

        public Vector2 Tangent(float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 tangentVector = -3 * uu * Points[0] +
                                    3 * (2 * uu - 3 * u) * (Points[1] - Points[2]) +
                                    3 * (3 * t - 2) * (Points[2] - Points[3]) +
                                    3 * ttt * (Points[3] - Points[2]);
            tangentVector.Normalize();
            return tangentVector;
        }

        public ICurve Reverse()
        {
            return new BezierCurve(Points.Reverse().ToArray());
        }
    }
}