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
            IsVariable = containsVariable;

            if (stringListValue.Count == 1 && Double.TryParse(stringListValue[0], out Double result))
            {
                NumberValue = result;
            }
        }

        public ASTreeNode LeftChild { get; set; }
        public ASTreeNode RightChild { get; set; }

        // public static List<Char> TerminalSymbols = new List<Char>()
        // {
        //     '+', '-', '*', '/', '(', ')'
        // };

        public Operations Operation { get; set; }
        public Boolean IsNumber => NumberValue != null;
        public Boolean IsLeaf { get; set; }

        public Boolean IsVariable { get; set; }

        public Double? NumberValue { get; set; }
        public List<String> StringListValue { get; set; }

        public List<String> GetExpression()
        {
            var result = Calculate();
            Boolean readyToUse = !canBeSimlified(result);
            while (!readyToUse) readyToUse = !canBeSimlified(result);
            return result;
        }

        public List<String> Calculate()
        {
            if (LeftChild == null && RightChild == null)
            {
                return StringListValue;
            }

            if (LeftChild.IsLeaf && RightChild.IsLeaf)
            {
                if (LeftChild.IsVariable || RightChild.IsVariable)
                {
                    var result = new List<String>();
                    switch (Operation)
                    {
                        case Operations.Add:
                            result.Add("(");
                            result.AddRange(LeftChild.StringListValue);
                            result.Add("+");
                            result.AddRange(RightChild.StringListValue);
                            result.Add(")");
                            return result;
                        case Operations.Subtract:
                            result = new List<String>();
                            result.Add("(");
                            result.AddRange(LeftChild.StringListValue);
                            result.Add("-");
                            result.AddRange(RightChild.StringListValue);
                            result.Add(")");
                            return result;
                        case Operations.Multiply:
                            result = new List<String>();
                            result.AddRange(LeftChild.StringListValue);
                            result.Add("*");
                            result.AddRange(RightChild.StringListValue);
                            return result;
                        case Operations.Divide:
                            result = new List<String>();
                            result.AddRange(LeftChild.StringListValue);
                            result.Add("/");
                            result.AddRange(RightChild.StringListValue);
                            return result;
                        default:
                            throw new UnacceptableExpressionException("Undefined operation");
                    }
                }
                else
                {
                    switch (Operation)
                    {
                        case Operations.Add:
                            Double? result = LeftChild.NumberValue + RightChild.NumberValue;
                            return new List<String>() {result.ToString()};
                        case Operations.Subtract:
                            result = LeftChild.NumberValue - RightChild.NumberValue;
                            return new List<String>() {result.ToString()};
                        case Operations.Multiply:
                            result = LeftChild.NumberValue * RightChild.NumberValue;
                            return new List<String>() {result.ToString()};
                        case Operations.Divide:
                            result = LeftChild.NumberValue / RightChild.NumberValue;
                            return new List<String>() {result.ToString()};
                        default:
                            throw new UnacceptableExpressionException("Undefined operation");
                    }
                }
            }
            else
            {
                var leftResult = LeftChild.Calculate();
                var rightResult = RightChild.Calculate();
                var finalResult = new List<String>();

                if (Operation == Operations.Add || Operation == Operations.Subtract)
                {
                    Boolean readyToUse = !canBeSimlified(leftResult);
                    while (!readyToUse) readyToUse = !canBeSimlified(leftResult);

                    readyToUse = !canBeSimlified(rightResult);
                    while (!readyToUse) readyToUse = !canBeSimlified(rightResult);
                }

                if (leftResult.Count == 1
                    && rightResult.Count == 1
                    && Double.TryParse(leftResult[0], out Double leftValue)
                    && Double.TryParse(rightResult[0], out Double rightValue))
                {
                    switch (Operation)
                    {
                        case Operations.Add:
                            finalResult.Add((leftValue + rightValue).ToString());
                            break;
                        case Operations.Subtract:
                            finalResult.Add((leftValue - rightValue).ToString());
                            break;
                        case Operations.Multiply:
                            finalResult.Add((leftValue * rightValue).ToString());
                            break;
                        case Operations.Divide:
                            finalResult.Add((leftValue / rightValue).ToString());
                            break;
                        default:
                            throw new UnacceptableExpressionException("Undefined operation");
                    }

                    return finalResult;
                }
                else
                {
                    finalResult = new List<string>();
                    finalResult.Add("(");
                    finalResult.AddRange(leftResult);
                    switch (Operation)
                    {
                        case Operations.Add:
                            finalResult.Add("+");
                            break;
                        case Operations.Subtract:
                            finalResult.Add("-");
                            break;
                        case Operations.Multiply:
                            finalResult.Add("*");
                            break;
                        case Operations.Divide:
                            finalResult.Add("/");
                            break;
                        default:
                            throw new UnacceptableExpressionException("Undefined operation");
                    }

                    finalResult.AddRange(rightResult);
                    finalResult.Add(")");
                    switch (Operation)
                    {
                        case Operations.Multiply:
                            finalResult.RemoveAt(0);
                            finalResult.RemoveAt(finalResult.Count - 1);
                            break;
                        case Operations.Divide:
                            finalResult.RemoveAt(0);
                            finalResult.RemoveAt(finalResult.Count - 1);
                            break;
                    }

                    return finalResult;
                }
            }
        }

        private Boolean canBeSimlified(List<String> input)
        {
            if (input == null
                || !(input[0] == "(" && input[^1] == ")")
            ) return false;

            UInt16 countOpened = 0;
            for (int i = 0; i < input.Count; ++i)
            {
                if (input[i] == "(") ++countOpened;
                if (input[i] == ")") --countOpened;
                if (countOpened == 0 && i != input.Count - 1) return false;
            }

            input.RemoveAt(0);
            input.RemoveAt(input.Count - 1);
            return true;
        }
    }
}