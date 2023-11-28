using UnityEngine;
namespace Geom
{
    public class PointRegion : IRegion
    {
        private readonly Vector2 _point;

        public PointRegion(Vector2 point)
        {
            _point = point;
        }

        Vector2 IRegion.CenterPoint => _point;

        float IRegion.Area => 0;

        public Vector2 RandPoint()
        {
            return _point;
        }
    }
}