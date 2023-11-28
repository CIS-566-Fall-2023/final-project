using System.Collections.Generic;
using UnityEngine;

namespace BinaryPartition
{
    public struct Rectangle
    {
        public Vector2 Min;
        public Vector2 Max;

        public static Rectangle UnitSquare()
        {
            return new Rectangle { Min = new Vector2(-1, -1), Max = new Vector2(1,1) };
        }

        public float GetDim(int dim)
        {
            return Max[dim] - Min[dim];
        }

        public List<Vector2> getPoints()
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(Min[0], Min[1]));
            points.Add(new Vector2(Min[0], Max[1]));

            points.Add(new Vector2(Min[0], Max[1]));
            points.Add(new Vector2(Max[0], Max[1]));

            points.Add(new Vector2(Max[0], Max[1]));
            points.Add(new Vector2(Max[0], Min[1]));

            points.Add(new Vector2(Max[0], Min[1]));
            points.Add(new Vector2(Min[0], Min[1]));

            return points;
        }
    }
    
}