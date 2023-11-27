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
        static readonly HashSet<string> OPERATORS = new HashSet<string> { "(", ")", "==", "!=", "&&", "||"/*, ">", "<", ">=", "<="*/ };
        struct Op
        {
            public enum Type { Operator, String, Integer };
            public Type type;
            public string strVal;
            public int intVal;
            public int Level
            {
                get
                {
                    if (type != Type.Operator || strVal == ")" || strVal == "(") throw new NotImplementedException();
                    switch (this.strVal)
                    {
                        default:
                        //case "(":
                        //    return 0;
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
                        case "!":
                            return 5;
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
        public void InitializeRPN()
        {
            var types = new HashSet<string>(WFCTypeStrings.strings);
            var parts = Regex.Split(rule, PATTERN);
            var infix = new List<Op>();
            try
            {
                foreach (var part in parts)
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        Op op = new Op();
                        if (OPERATORS.Contains(part))
                        {
                            op.type = Op.Type.Operator;
                            op.strVal = part;
                        }
                        else if (types.Contains(part))
                        {
                            op.type = Op.Type.String;
                            op.strVal = part;
                        }
                        else
                        {
                            op.type = Op.Type.Integer;
                            op.intVal = int.Parse(part);
                        }
                        infix.Add(op);
                    }
                    // shunting yard
                    var it = infix.GetEnumerator();
                    var operators = new Stack<Op>();
                    do
                    {
                        var curr = it.Current;
                        if (curr.type == Op.Type.String || curr.type == Op.Type.Integer)
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
                    } while (it.MoveNext());
                    while (operators.Count > 0) RPN.Add(operators.Pop());
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"WFC rule \"{rule}\" has errors.");
                RPN = null;
            }
        }
        public bool ApplyRule(ref float val, IWFCCell[] neibours)
        {
            bool condition = true;
            if (rule != null && rule != "")
            {
                if (RPN == null)  InitializeRPN();
                if (RPN != null)
                {

                }
            }
            if (condition) Apply(ref val);
            return true;
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
