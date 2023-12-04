using System;
using System.Collections.Generic;
using System.Linq;
using Geom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation.Tower
{
    public class TowerRunner
    {
        private const float Trim = 2;
        private BuildingGenerator _generator;
        private const float MinTheta = (float)(Math.PI / 16);
        private const float MaxTheta = (float)(Math.PI / 2);

        public List<Sector> OuterRooms = new();

        public TowerRunner(Vector2 center, float radiusInner, float hallRadius, float radiusOuter)
        {
            var theta = (float)(Random.value * 2 * Math.PI);
            var outerRoom = new Sector
            {
                Center = center,
                RadiusOuter = radiusOuter,
                RadiusInner = hallRadius + Trim
            };

            var outerWalls = SpacedAngles().ToList();
            for (int i = 0; i < outerWalls.Count - 1; i++)
            {
                outerRoom.Theta0 = outerWalls[i];
                outerRoom.Theta1 = outerWalls[(i + 1) % outerWalls.Count];
                OuterRooms.Add(outerRoom);
            }
        }

        private IEnumerable<float> SpacedAngles()
        {
            Stack<(float, float)> s = new();
            var theta = (float)(Random.value * 2 * Math.PI);
            s.Push((theta, (float)(theta + 2 * Math.PI)));

            while (s.Count != 0)
            {
                var (left, right) = s.Pop();
                var length = right - left;
                if (length > MaxTheta ||
                    (length > 2 * MinTheta && Random.value > 0.5))
                {
                    theta = Mathf.Lerp(left + MinTheta, right + MinTheta, Random.value);
                    s.Push((theta, right));
                    s.Push((left, theta));
                }
                else
                {
                    yield return left;
                }
            }
        }
    }
}