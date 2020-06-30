using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    public class ASTreeNode
    {
        public ASTreeNode(List<String> stringListValue)
        {
            StringListValue = stringListValue;

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

        public Boolean IsNumber => NumberValue != null;

        public Boolean ContainsVariable()
        {
            foreach (var token in StringListValue)
            {
                if (token.Length > 1
                    && !Int32.TryParse(token, out Int32 result)
                    || !TerminalSymbols.Contains(token[0])
                ) return true;
            }

            return false;
        }

        public Int32? NumberValue { get; private set; }
        public List<String> StringListValue { get; private set; }
    }
}