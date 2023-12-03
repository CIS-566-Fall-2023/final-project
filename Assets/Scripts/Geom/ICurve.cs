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

        IEnumerable<Vector2> ToPointStream(float delta = 1)
        {
            var length = Length();
            var segmentCount = (int)Mathf.Ceil(delta * length);
            var segmentLength = length / segmentCount;
            for (var i = 0; i < segmentCount; i++)
            {
                yield return Point(i * segmentLength);
            }
        }
    }
    

}