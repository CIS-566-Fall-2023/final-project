using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace Planetile
{
    [Serializable]
    public struct WFCRule
    {
        const string PATTERN = @"(==|!=|&&|\|\||\(|\))";
        static readonly HashSet<string> OPERATORS = new HashSet<string> { "(", ")", "==", "!=", "&&", "||"/*, ">", "<", ">=", "<=", "!" */ };
        struct Op
        {
            public enum Type { Operator, WFCType, Integer };
            public Type type;
            public string strVal;
            public int intVal;
            public WFCType typeVal;
            public int Level
            {
                get
                {
                    if (type != Type.Operator || strVal == ")") throw new NotImplementedException();
                    switch (this.strVal)
                    {
                        default:
                        case "(":
                            return 0;
                        case "||":
                            return 1;
                        case "&&":
                            return 2;
                        case "==":
                            return 3;
                        case "!=":
                            return 3;
                        //case ">":
                        //    return 4;
                        //case ">=":
                        //    return 4;
                        //case "<":
                        //    return 4;
                        //case "<=":
                        //    return 4;
                        //case "!":
                        //    return 5;
                    }
                }
            }
        }
        public enum Operation
        {
            ADD = 0x01,
            MULTIPLY = 0x01 << 2,
            ASSIGN = 0x01 << 3,
        }
        public string rule;
        public Operation operation;
        public float value;
        List<Op> RPN;
        public bool Initialized
        {
            get => RPN != null && RPN.Count > 0;
        }
        void Apply(ref float val)
        {
            switch (operation)
            {
                default:
                case Operation.ADD:
                    val += value;
                    break;
                case Operation.MULTIPLY:
                    val *= value;
                    break;
                case Operation.ASSIGN:
                    val = value;
                    break;
            }
            return;
        }
        public bool InitializeRPN()
        {
            var types = new HashSet<string>(Enum.GetNames(typeof(WFCType)));
            var parts = Regex.Split(rule, PATTERN);
            var infix = new List<Op>();
            try
            {
                foreach (var part in parts)
                {
                    string str = part.Replace(" ", "");
                    if (!string.IsNullOrEmpty(str))
                    {
                        Op op = new Op();
                        if (OPERATORS.Contains(str))
                        {
                            op.type = Op.Type.Operator;
                            op.strVal = str;
                        }
                        else if (types.Contains(str))
                        {
                            op.type = Op.Type.WFCType;
                            op.typeVal = (WFCType)Enum.Parse(typeof(WFCType), str);
                        }
                        else
                        {
                            op.type = Op.Type.Integer;
                            op.intVal = int.Parse(str);
                        }
                        infix.Add(op);
                    }
                }
                if (infix.Count == 0) { return false; }
                RPN = new List<Op>();
                // shunting yard
                var it = infix.GetEnumerator();
                var operators = new Stack<Op>();
                while (it.MoveNext())
                {
                    var curr = it.Current;
                    if (curr.type == Op.Type.WFCType || curr.type == Op.Type.Integer)
                        RPN.Add(curr);
                    else if (curr.strVal == "(")
                    {
                        operators.Push(curr);
                    }
                    else if (curr.strVal == ")")
                    {
                        while (true)
                        {
                            var op = operators.Pop();
                            if (op.strVal == "(") break;
                            RPN.Add(op);
                        }
                    }
                    else
                    {
                        while (operators.Count > 0 && operators.First().Level >= curr.Level) RPN.Add(operators.Pop());
                        operators.Push(curr);
                    }
                } 
                while (operators.Count > 0) RPN.Add(operators.Pop());
            }
            catch (Exception e)
            {
                Debug.LogError($"WFC rule \"{rule}\" has errors.");
                RPN = null;
                return false;
            }
            return true;
        }
        public bool SatisfyRule(IWFCCell[] neighbors)
        {
            var operends = new Stack<WFCType>();
            var conditions = new Stack<bool>();
            foreach (Op op in RPN)
            {
                if (op.type == Op.Type.WFCType)
                {
                    operends.Push(op.typeVal);
                }
                if (op.type == Op.Type.Integer)
                {
                    if (op.intVal < neighbors.Length)
                        operends.Push(neighbors[op.intVal] != null? neighbors[op.intVal].Type : WFCType.Null);
                    else
                        operends.Push(WFCType.Null);
                }
                else
                {
                    switch (op.strVal)
                    {
                        case "==":
                            var a = operends.Pop();
                            var b = operends.Pop();
                            conditions.Push((a & b) != 0 || a == WFCType.Null || b == WFCType.Null);
                            break;
                        case "!=":
                            a = operends.Pop();
                            b = operends.Pop();
                            conditions.Push((a & b) == 0);
                            break;
                        case "&&":
                            bool c = conditions.Pop();
                            bool d = conditions.Pop();
                            conditions.Push(c && d);
                            break;
                        case "||":
                            c = conditions.Pop();
                            d = conditions.Pop();
                            conditions.Push(c || d);
                            break;
                        default:
                            break;
                    }
                }
            }
            return conditions.First();
        }
        public bool ApplyRule(ref float val, IWFCCell[] neighbors)
        {
            bool result = true;
            if (rule != null && rule != "")
            {
                if (RPN == null)  InitializeRPN();
                if (RPN != null)
                {
                    result = SatisfyRule(neighbors);
                }
            }
            if (result) Apply(ref val);
            return result;
        }
    }
    public interface IWFCItem
    {
        string ItemName { get; }
        WFCType Type { get; }
        WFCRule[] Rules { get; }
        float Entropy(IWFCCell cell);
    }
}
