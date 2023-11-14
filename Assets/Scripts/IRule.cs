using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public interface IRule
    {
        float Entropy(Cell[] cells);
    }
}
