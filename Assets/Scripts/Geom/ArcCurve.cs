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

        public float TangentAngle(float t)
        {
            var theta = Mathf.Lerp(Theta0, Theta1, t);
            return (Theta1 > Theta0) ? theta : -theta;
        }

        public (ICurve, Vector2, ICurve) Split(float t)
        {
            var point = Point(t);
            var theta = Mathf.Lerp(Theta0, Theta1, t);
            return (
                new ArcCurve{Theta0 = Theta0, Theta1 = theta, Radius = Radius, Center = Center},
                point,
                new ArcCurve{Theta0 = theta, Theta1 = Theta1, Radius = Radius, Center = Center});
        }

        public ICurve Reverse()
        {
            return new ArcCurve { Theta0 = Theta1, Theta1 = Theta0, Radius = Radius, Center = Center };
        }
    }
}