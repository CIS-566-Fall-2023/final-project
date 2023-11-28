using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BinaryPartition
{
    public class Divider : IComparable<Divider>
    {
        private const float EdgeGap = 1;
        private readonly bool isHorizontal;
        public float AxisValue { get; }
        private readonly float _start;
        private readonly float _end;
        private readonly List<Divider> _below = new();
        private readonly List<Divider> _above = new();

        public Divider(float axisValue, int parAxis, Rectangle rectangle)
        {
            AxisValue = axisValue;
            isHorizontal = parAxis == 0;
            _start = rectangle.Min[parAxis];
            _end = rectangle.Max[parAxis];
        }

        public void AddBelow(Divider divider)
        {
            _below.Add(divider);
        }

        public void AddAbove(Divider divider)
        {
            _above.Add(divider);
        }

        public IEnumerable<(Vector2, Vector2)> GetSegments()
        {
            var ax = AxisValue;
            Func<float, Vector2> toPoint = isHorizontal ?
                n => new Vector2(n, ax) :
                n => new Vector2(ax, n);

            List<Divider> incidentDividers = Utils.Merge(_above, _below);
            
            if (incidentDividers.Count == 0)
            {
                return new[] {(toPoint(_start), toPoint(_end))};
            }

            var segStarts =
                new[] {_start}
                    .Concat(incidentDividers.Select(divider => divider.AxisValue))
                    .Select(v => v + EdgeGap);

            var segEnds =
                incidentDividers.Select(divider => divider.AxisValue)
                    .Concat(new[] {_end})
                    .Select(v => v).ToList();

            var segments = segStarts.Zip(segEnds,
                (a, b) => (toPoint(a + EdgeGap), toPoint(b - EdgeGap)));

            return incidentDividers.Aggregate(segments,
                (current, divider) => current.Concat(divider.GetSegments()));
        }
        public int CompareTo(Divider other)
        {
            return AxisValue.CompareTo(other.AxisValue);
        }
    }
}