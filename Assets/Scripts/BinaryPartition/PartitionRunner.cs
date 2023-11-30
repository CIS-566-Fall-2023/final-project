using System.Collections.Generic;
using GraphBuilder;

namespace BinaryPartition
{
    public class PartitionRunner
    {
        private readonly List<Divider> _dividers = new();
        public readonly Builder Builder;
        private readonly BinaryBlock _root;

        public PartitionRunner(Builder builder, Rectangle rectangle)
        {
            Builder = builder;
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