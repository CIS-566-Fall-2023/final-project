using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    [Serializable]
    public struct WFCRule
    {
        public enum Operation
        {
            ADD = 0x01,
            SUBTRACT = 0x01 << 2,
            MULTIPLY = 0x01 << 3,
            ASSIGN = 0x01 << 4,
        }
        public string rule;
        public Operation operation;
        public float value;
    }
    public interface IWFCItem
    {
        WFCType Type { get; }
        WFCRule Rule { get; }
        float Entropy(IWFCCell cell);
    }
}
