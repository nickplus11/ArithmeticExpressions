using System;
using System.Collections.Generic;

public enum Operations
{
    Add,
    Subtract,
    Divide,
    Multiply,
    NotDefined
}

namespace ArithmeticExpressions
{
    public class ASTreeNode
    {
        public ASTreeNode(List<String> stringListValue, Operations operation,
            Boolean isLeaf = false, Boolean containsVariable = false)
        {
            StringListValue = stringListValue;
            Operation = operation;
            IsLeaf = isLeaf;
            ContainsVariable = containsVariable;

            if (stringListValue.Count == 1 && Int32.TryParse(stringListValue[0], out Int32 result))
            {
                NumberValue = result;
            }
        }

        public ASTreeNode LeftChild { get; set; }
        public ASTreeNode RightChild { get; set; }

        public static List<Char> TerminalSymbols = new List<Char>()
        {
            '+', '-', '*', '/', '(', ')'
        };

        public Operations Operation { get; set; }
        public Boolean IsNumber => NumberValue != null;
        public Boolean IsLeaf { get; set; }

        public Boolean ContainsVariable { get; private set; }

        public Int32? NumberValue { get; set; }
        public List<String> StringListValue { get; private set; }
    }
}