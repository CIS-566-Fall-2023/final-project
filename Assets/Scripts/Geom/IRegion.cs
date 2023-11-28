using UnityEngine;

namespace Geom
{
    public interface IRegion
    {
        Vector2 CenterPoint { get; }
        float Area { get;  }
        Vector2 RandPoint();
    }
}