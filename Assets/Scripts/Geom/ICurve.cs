using System.Collections.Generic;
using UnityEngine;

namespace Geom
{
    public interface ICurve
    {
        float Length();
        Vector2 Point(float t);
        (ICurve, Vector2, ICurve) Split(float t);
        ICurve Reverse();

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