using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public interface IWFCItem
    {
        float Entropy(IWFCCell[] cells);
    }
}
