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
    }
    
}