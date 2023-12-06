using System.Collections.Generic;
using UnityEngine;

namespace Geom
{
    public interface ICurve
    {
        float Length();
        Vector2 Point(float t);

        Vector2 Tangent(float t);

        public ICurve Reverse();

        public float TangentAngle(float t)
        {
            Vector2 tangent = Tangent(t);
            return Mathf.Atan2(tangent.y, tangent.x);
        }

        public IEnumerable<Vector2> ToPointStream(float deltaLength = 10)
        {
            var length = Length();
            var segmentCount = (int)Mathf.Ceil(length / deltaLength);
            var deltaT = 1.0f / segmentCount;
            for (var i = 0; i <= segmentCount; i++)
            {
                yield return Point(i * deltaT);
            }
        }
    }
    

}