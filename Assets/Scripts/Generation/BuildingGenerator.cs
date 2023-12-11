using System.Collections.Generic;
using BinaryPartition;
using Generation.Tower;
using Geom;
using GraphBuilder;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Generation
{
    public class BuildingGenerator
    {
        public readonly Builder Builder = new();
        private readonly List<ICurve> _walls = new();

        public void AddWall(ICurve curve)
        {
            _walls.Add(curve);
        }

        public IEnumerable<ICurve> GetWalls()
        {
            return _walls;
        }

        public void GenerateBuilding()
        {
            var partitionRunner = new PartitionRunner(this, new Rectangle
            {
                Min = new Vector2(-130, -60), Max = new Vector2(0, 60)
            });
            partitionRunner.Run();
            new TowerRunner(this, new Vector2(75, 0), 10, 12);
        }
    }
}