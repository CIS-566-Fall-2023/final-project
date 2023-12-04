using UnityEngine;

namespace Geom
{
    public class Sector
    {
        public Vector2 Center;
        public float RadiusInner;
        public float RadiusOuter;
        public float Theta0;
        public float Theta1;

        public float GetLength()
        {
            return Theta1 - Theta0;
        }
    }
}