using System.Collections.Generic;
using BinaryPartition;
using Geom;
using GraphBuilder;
using Vector2 = UnityEngine.Vector2;

namespace Generation
{
    public class BuildingGenerator
    {
        public Builder Builder = new();
        private List<ICurve> _walls = new();

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
            var runner = new PartitionRunner(this, new Rectangle
            {
                Min = new Vector2(-100, -50), Max = new Vector2(100, 50)
            });
            runner.Run();
        }
    }
}