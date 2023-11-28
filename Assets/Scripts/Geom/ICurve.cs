using UnityEngine;

namespace Geom
{
    public interface ICurve
    {
        float Length();
        Vector2 Point(float t);
        (ICurve, Vector2, ICurve) Split(float t);
        ICurve Reverse();
    }
}