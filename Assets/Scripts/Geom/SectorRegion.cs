using UnityEngine;

namespace Geom
{
    public class SectorRegion : IRegion
    {
        public Sector Sector;

        public SectorRegion(Sector sector)
        {
            Sector = sector;
        }

        public Vector2 CenterPoint
        {
            get
            {
                var theta = (Sector.Theta0 + Sector.Theta1) / 2;
                var radius = (Sector.RadiusOuter + Sector.RadiusInner) / 2;
                return Sector.Center + radius * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            }
        }

        public float Area => (Sector.Theta1 - Sector.Theta0) * 
                             (Sector.RadiusOuter * Sector.RadiusOuter - Sector.RadiusInner * Sector.RadiusInner);

        public Vector2 RandPoint()
        {
            var theta = Mathf.Lerp(Sector.Theta0, Sector.Theta1, Random.value);
            var radius = Mathf.Lerp(Sector.RadiusInner, Sector.RadiusInner, Random.value);
            return Sector.Center + radius * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }
    }
}