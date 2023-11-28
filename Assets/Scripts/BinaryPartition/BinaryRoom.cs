#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BinaryPartition
{
    public class BinaryRoom
    {
        private static readonly Vector2 MinDims = new Vector2(8, 8);
        private static readonly Vector2 MaxDims = new Vector2(50, 50);
        private const float TrimSize = 1;

        private Rectangle _rectangle;

        private struct DividerBounds
        {
            public Divider? Low;
            public Divider? High;
        }
        
        private DividerBounds[] _dividers =
        {
            new() {Low = null, High = null},
            new() {Low = null, High = null}
        };
        
        public Divider? SplitDivider;

        private BinaryRoom? _leftChild;
        private BinaryRoom? _rightChild;
        private bool IsLeaf
        {
            get => _leftChild == null && _rightChild == null; 
        }
        
        public BinaryRoom(Rectangle rectangle)
        {
            _rectangle = rectangle;
        }

        private BinaryRoom()
        {
            
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
        }

        public List<Rectangle> GetRects()
        {
            List<Rectangle> res = new ();
            AppendRects(res);
            return res;
        }

        public void AppendRects(ICollection<Rectangle> list)
        {
            if (IsLeaf)
            {
                list.Add(new Rectangle
                {
                    Max = _rectangle.Max - new Vector2(TrimSize, TrimSize),
                    Min = _rectangle.Min + new Vector2(TrimSize, TrimSize)
                });
                return;
            }
            _leftChild?.AppendRects(list);
            _rightChild?.AppendRects(list);
        }

        private void SplitPerpToAxis(int perpAxis)
        {
            var parAxis = 1 - perpAxis;
            
            var v = Mathf.Lerp(
                _rectangle.Min[perpAxis] + MinDims[perpAxis], 
                _rectangle.Max[perpAxis] - MinDims[perpAxis], 
                Random.value);
            SplitDivider = new Divider(v, parAxis, _rectangle);
            
            var leftRect = _rectangle;
            leftRect.Max[perpAxis] = v;
            _leftChild = new BinaryRoom
            {
                _rectangle = _rectangle,
                _dividers = _dividers.ToArray()
            };
            _leftChild._rectangle.Max[perpAxis] = v;
            _leftChild._dividers[perpAxis].High = SplitDivider;
            _leftChild.RandomSplit();

            _dividers[parAxis].Low?.AddBelow(SplitDivider);
            _dividers[parAxis].High?.AddAbove(SplitDivider);
            
            _rightChild = new BinaryRoom
            {
                _rectangle = _rectangle,
                _dividers = _dividers.ToArray()
            };
            _rightChild._rectangle.Min[perpAxis] = v;
            _rightChild._dividers[perpAxis].Low = SplitDivider;
            _rightChild.RandomSplit();
        }
    }
}