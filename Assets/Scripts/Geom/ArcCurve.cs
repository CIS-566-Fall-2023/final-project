using UnityEngine;

namespace Geom
{
    public class ArcCurve : ICurve
    {
        public float Theta0;
        public float Theta1;
        public float Radius;
        public Vector2 Center;
        public float Length()
        {
            return Radius * Mathf.Abs(Theta1 - Theta0);
        }

        public Vector2 Point(float t)
        {
            var theta = Mathf.Lerp(Theta0, Theta1, t);
            return Center + Radius * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }

        public Vector2 Tangent(float t)
        {
            var theta = Mathf.Lerp(Theta0, Theta1, t);
            var tangent = new Vector2(-Mathf.Sin(theta), Mathf.Cos(theta));
            if (Theta1 > Theta0) tangent *= -1;
            return tangent;
        }

        public ICurve Reverse()
        {
            return new ArcCurve { Theta0 = Theta1, Theta1 = Theta0, Radius = Radius, Center = Center };
        }
    }
}