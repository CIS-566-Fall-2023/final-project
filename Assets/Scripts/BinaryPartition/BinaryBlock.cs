#nullable enable
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BinaryPartition
{
    public class BinaryBlock
    {
        private readonly PartitionRunner _partitionRunner;
        private static readonly Vector2 MinDims = new Vector2(20, 20);
        private static readonly Vector2 MaxDims = new Vector2(50, 50);

        private Rectangle _rectangle;
        
        private DividerBounds[] _dividers =
        {
            new() {Low = null, High = null},
            new() {Low = null, High = null}
        };

        private Divider? _splitDivider;
        private BinaryBlock? _leftChild;
        private BinaryBlock? _rightChild;

        public BinaryBlock(PartitionRunner partitionRunner, Rectangle rectangle)
        {
            _partitionRunner = partitionRunner;
            _rectangle = rectangle;
        }

        private BinaryBlock MakeChild()
        {
            var child = new BinaryBlock(_partitionRunner, _rectangle)
            {
                _dividers = _dividers.ToArray()
            };
            return child;
        }

        public void RandomSplit()
        {
            var rand = Random.value;
            
            if (rand < 0.5f && _rectangle.GetDim(0) >= 2 * MinDims[0])
            {
                SplitPerpToAxis(0);
            }
            else if (_rectangle.GetDim(1) >= 2 * MinDims[1])
            {
                SplitPerpToAxis(1);
            }
            else if (_rectangle.GetDim(0) > MaxDims[0])
            {
                SplitPerpToAxis(0);
            }
            else if (_rectangle.GetDim(1) > MaxDims[1])
            {
                SplitPerpToAxis(1);
            }
            else
            {
                var room = new BlockRoom(_dividers, _rectangle, _partitionRunner.Builder);
                room.AddDoorways();
            }
        }

        private void SplitPerpToAxis(int perpAxis)
        {
            var parAxis = 1 - perpAxis;
            
            var v = Mathf.Lerp(
                _rectangle.Min[perpAxis] + MinDims[perpAxis], 
                _rectangle.Max[perpAxis] - MinDims[perpAxis], 
                Random.value);
            _splitDivider = new Divider(_partitionRunner, v, parAxis, _rectangle);
            
            var leftRect = _rectangle;
            leftRect.Max[perpAxis] = v;
            _leftChild = MakeChild();
            _leftChild._rectangle.Max[perpAxis] = v;
            _leftChild._dividers[perpAxis].High = _splitDivider;
            _leftChild.RandomSplit();

            _dividers[parAxis].Low?.AddBelow(_splitDivider.Start);
            _dividers[parAxis].High?.AddAbove(_splitDivider.End);
            
            _rightChild = MakeChild();
            _rightChild._rectangle.Min[perpAxis] = v;
            _rightChild._dividers[perpAxis].Low = _splitDivider;
            _rightChild.RandomSplit();
        }
    }
}