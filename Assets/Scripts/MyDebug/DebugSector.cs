using System;
using System.Collections.Generic;
using Geom;
using UnityEngine;

namespace MyDebug
{
    public class DebugSector : IDebugDrawable
    {
        private const float DTheta = (float)(Math.PI / 64);
        private readonly List<(Vector2, Vector2)> _segments = new();
        public Color Color;

        public DebugSector(Sector sector, Color color)
        {
            var inner = new ArcCurve {
                Theta0 = sector.Theta0,
                Theta1 = sector.Theta1,
                Center = sector.Center,
                Radius = sector.RadiusInner };
            
            foreach (var segment in ((ICurve)inner).ToPointStream().Pairwise())
            {
                _segments.Add(segment);
            }
            
            var outer = new ArcCurve {
                Theta0 = sector.Theta0,
                Theta1 = sector.Theta1,
                Center = sector.Center,
                Radius = sector.RadiusOuter };
            
            foreach (var segment in ((ICurve)outer).ToPointStream().Pairwise())
            {
                _segments.Add(segment);
            }
            
            _segments.Add((inner.Point(0), outer.Point(0)));
            _segments.Add((inner.Point(1), outer.Point(1)));

            Color = color;
        }
        public void Draw()
        {
            foreach (var (a, b) in _segments)
            {
                var seg = new DebugSegment { Color = Color, P0 = a, P1 = b };
                seg.Draw();
            }
            
        }
    }
}