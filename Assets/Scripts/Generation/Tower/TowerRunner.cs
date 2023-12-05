using System;
using System.Collections.Generic;
using System.Linq;
using Geom;
using GraphBuilder;
using Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation.Tower
{
    public class TowerRunner
    {
        private BuildingGenerator _generator;
        private const float WallThickness = 2;
        private const float OuterMinTheta = (float)(Math.PI / 8);
        private const float InnerMinTheta = (float)(Math.PI / 5);
        private const float DoorSize = 2;
        private const float DoorMargin = 4;
        private const float HallMargin = 6;

        private Vector2 _center;
        
        public TowerRunner(BuildingGenerator generator, Vector2 center, float centerRadius, float roomLength)
        {
            _generator = generator;
            _center = center;
            var sector = new Sector
            {
                Center = center,
                RadiusInner = centerRadius + roomLength,
                RadiusOuter = centerRadius + 2 * roomLength,
            };
            
            // Create inner rooms and doors
            var innerRooms = MakeRooms(sector, InnerMinTheta).ToList();
            MakeDoorsBetween(innerRooms, sector);
            var stairDoors = MakeDoorsInside(sector, innerRooms);
            var innerHallDoors = MakeDoorsOutside(sector, innerRooms);
            
            // Create outer rooms and doors
            sector.RadiusInner = sector.RadiusOuter + HallMargin;
            sector.RadiusOuter = sector.RadiusInner + roomLength;
            var outerRooms = MakeRooms(sector, OuterMinTheta).ToList();
            MakeDoorsBetween(outerRooms, sector);
            var outerHallDoors = MakeDoorsInside(sector, outerRooms);
            
            // Link doors to inner ring
            

            var hallDoors = innerHallDoors.Concat(outerHallDoors).Select(p =>
            {
                var (r, s) = p;
                return (r, s % Utils.Tau);
            }).ToList();
            hallDoors.Sort((x, y) => y.Item2.CompareTo(y.Item2));
            


        }

        private Vector2 ToCartesian(float r, float theta)
        {
            return _center + r * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }
        
        private List<(Sector, VertexId)> MakeRooms(Sector sector, float minTheta)
        {
            var wallAngles = SpacedAngles(minTheta).ToList();
            var dWall = WallThickness / sector.RadiusInner;
            
            return wallAngles.Pairwise().Select(bounds =>
            {
                var (t0, t1) = bounds;
                sector.Theta0 = t0 + dWall;
                sector.Theta1 = t1 - dWall;
                var vertexId = _generator.Builder.MakeVertex(new VertexInfo(new SectorRegion(sector), VertexTag.Room));
                Debug.Log(vertexId.Id);
                return (sector, vertexId);
            }).ToList();
        }

        private void MakeDoorsBetween(List<(Sector, VertexId)> rooms, Sector sector)
        {
            var doorRadius = (sector.RadiusInner + sector.RadiusOuter) / 2;

            var (sLast, rLast) = rooms.First();
            sLast.Theta0 += Utils.Tau;
            
            foreach (var ((s0, r0), (s1, r1)) in
                     rooms.Pairwise().Concat(new [] {(rooms.Last(), (sLast, rLast))}))
            {
                var curve = new ArcCurve{Center = sector.Center, Radius = doorRadius, Theta0 = s0.Theta1, Theta1 = s1.Theta0};
                _generator.Builder.MakeEdge(r0, r1, EdgeTag.Hallway, curve);
                var radii = new[] {
                    sector.RadiusInner, doorRadius - DoorSize, doorRadius + DoorSize, sector.RadiusOuter
                };
                var leftPoints = radii.Select(r => ToCartesian(r, curve.Theta0)).ToList();
                var rightPoints = radii.Select(r => ToCartesian(r, curve.Theta1)).ToList();
                _generator.AddWall(new LineCurve(leftPoints[0], leftPoints[1]));
                _generator.AddWall(new LineCurve(leftPoints[2], leftPoints[3]));
                _generator.AddWall(new LineCurve(rightPoints[0], rightPoints[1]));
                _generator.AddWall(new LineCurve(rightPoints[2], rightPoints[3]));
                _generator.AddWall(new LineCurve(leftPoints[1], rightPoints[1]));
                _generator.AddWall(new LineCurve(leftPoints[2], rightPoints[2]));
            }
        }

        private List<(VertexId, float)> MakeDoorsOutside(Sector sector, List<(Sector, VertexId)> rooms)
        {
            var radialDoorSize = DoorSize / sector.RadiusOuter;
            var radialDoorMargin = DoorMargin / sector.RadiusOuter;
            List<(VertexId, float)> doorPositions = new();
            
            foreach (var (s, r) in rooms)
            {
                var pos = Mathf.Lerp(s.Theta0 + radialDoorMargin, s.Theta1 - radialDoorMargin, Random.value);
                doorPositions.Add((r, pos));

                var leftTheta = pos - radialDoorSize;
                var rightTheta = pos + radialDoorSize;
                
                _generator.AddWall(new ArcCurve
                {
                    Center = sector.Center,
                    Radius = sector.RadiusOuter,
                    Theta0 = s.Theta0,
                    Theta1 = leftTheta
                });
                
                _generator.AddWall(new ArcCurve
                {
                    Center = sector.Center,
                    Radius = sector.RadiusOuter,
                    Theta0 = rightTheta,
                    Theta1 = s.Theta1
                });
                
                _generator.AddWall(new LineCurve(
                    ToCartesian(sector.RadiusOuter, leftTheta),
                    ToCartesian(sector.RadiusOuter + WallThickness * 2, leftTheta)
                ));
                
                _generator.AddWall(new LineCurve(
                    ToCartesian(sector.RadiusOuter, rightTheta),
                    ToCartesian(sector.RadiusOuter + WallThickness * 2, rightTheta)
                ));
            }

            foreach (var (theta0, theta1) in doorPositions.Select(p => p.Item2).PairwiseRing())
            {
                _generator.AddWall(new ArcCurve
                {
                    Center = sector.Center,
                    Radius = sector.RadiusOuter + WallThickness * 2,
                    Theta0 = theta0 + radialDoorSize,
                    Theta1 = theta1 - radialDoorSize
                });
            }

            return doorPositions;
        }
        
        private List<(VertexId, float)> MakeDoorsInside(Sector sector, List<(Sector, VertexId)> rooms)
        {
            var radialDoorSize = DoorSize / sector.RadiusInner;
            var radialDoorMargin = DoorMargin / sector.RadiusInner;
            List<(VertexId, float)> doorPositions = new();
            
            foreach (var (s, r) in rooms)
            {
                var theta = Mathf.Lerp(s.Theta0 + radialDoorMargin, s.Theta1 - radialDoorMargin, Random.value);
                var hallVertex = _generator.Builder.MakeVertex(ToCartesian(sector.RadiusInner - HallMargin, theta));
                
                doorPositions.Add((hallVertex, theta));

                var leftTheta = theta - radialDoorSize;
                var rightTheta = theta + radialDoorSize;
                
                _generator.AddWall(new ArcCurve
                {
                    Center = sector.Center,
                    Radius = sector.RadiusInner,
                    Theta0 = s.Theta0,
                    Theta1 = leftTheta
                });
                
                _generator.AddWall(new ArcCurve
                {
                    Center = sector.Center,
                    Radius = sector.RadiusInner,
                    Theta0 = rightTheta,
                    Theta1 = s.Theta1
                });
                
                _generator.AddWall(new LineCurve(
                    ToCartesian(sector.RadiusInner, leftTheta),
                    ToCartesian(sector.RadiusInner - WallThickness * 2, leftTheta)
                ));
                
                _generator.AddWall(new LineCurve(
                    ToCartesian(sector.RadiusInner, rightTheta),
                    ToCartesian(sector.RadiusInner - WallThickness * 2, rightTheta)
                ));
            }

            foreach (var (theta0, theta1) in doorPositions.Select(p => p.Item2).PairwiseRing())
            {
                _generator.AddWall(new ArcCurve
                {
                    Center = sector.Center,
                    Radius = sector.RadiusInner - WallThickness * 2,
                    Theta0 = theta0 + radialDoorSize,
                    Theta1 = theta1 - radialDoorSize
                });
            }

            return doorPositions;
        }

        private IEnumerable<float> SpacedAngles(float minTheta)
        {
            Stack<(float, float)> s = new();
            var theta = Random.value * Utils.Tau;
            s.Push((theta, theta + Utils.Tau));

            while (s.Count != 0)
            {
                var (left, right) = s.Pop();
                var length = right - left;
                if (length > 2 * minTheta)
                {
                    theta = Mathf.Lerp(left + minTheta, right - minTheta, Random.value);
                    s.Push((theta, right));
                    s.Push((left, theta));
                }
                else
                {
                    yield return left;
                    if (s.Count == 0)
                    {
                        yield return right;
                    }
                }
            }
        }
    }
}