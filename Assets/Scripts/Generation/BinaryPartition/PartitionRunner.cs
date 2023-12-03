using System.Collections.Generic;
using Generation;

namespace BinaryPartition
{
    public class PartitionRunner
    {
        private readonly List<Divider> _dividers = new();
        public readonly BuildingGenerator Generator;
        private readonly BinaryBlock _root;

        public PartitionRunner(BuildingGenerator generator, Rectangle rectangle)
        {
            Generator = generator;
            _root = new BinaryBlock(this, rectangle);
        }

        public void AddDivider(Divider divider)
        {
            _dividers.Add(divider);
        }

        public void Run()
        {
            _root.RandomSplit();
            foreach (var divider in _dividers)
            {
                divider.MakeEdges();
            }
        }

    }
}