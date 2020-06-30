using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ArithmeticExpressions
{
    public class ASTreeCreator
    {
        public List<String> InputStringList { get; private set; }

        public ASTreeNode Root { get; private set; }

        public ASTreeCreator() : this(new List<String>())
        {
        }

        public ASTreeCreator(List<String> inputStringList)
        {
            InputStringList = inputStringList;
            Root = new ASTreeNode(InputStringList, Operations.NotDefined);
        }

        public void BuildASTree()
        {
            useGrammarTerm(InputStringList, Root);
        }

        private void useGrammarTerm(List<String> tokens, ASTreeNode node)
        {
            if (InputStringList.Count == 0) throw new UnacceptableExpressionException("Empty expression");
            Boolean readyToUse = !canBeSimlified(tokens);
            while (!readyToUse) readyToUse = !canBeSimlified(tokens);
            useGrammarAdd(tokens, node);
        }

        private void useGrammarAdd(List<String> tokens, ASTreeNode node)
        {
            var leftPart = new List<string>();
            var rightPart = new List<string>();
            UInt16 index = 0;
            Operations operation = Operations.NotDefined;

            while (index < tokens.Count)
            {
                UInt16 stepIndex = index;
                UInt16 openBrackets = 0;
                if (tokens[index] == "(")
                {
                    ++openBrackets;
                    ++index;
                    while (openBrackets > 0)
                    {
                        if (tokens[index] == "(") ++openBrackets;

                        leftPart.Add(tokens[index]);
                        ++index;
                        if (index >= tokens.Count)
                            throw new UnacceptableExpressionException("Absence of a closing bracket");
                        if (tokens[index] == ")") --openBrackets;
                    }

                    ++index;
                    if (index >= tokens.Count) break;

                    if (!(tokens[index] == "+" || tokens[index] == "-"))
                    {
                        leftPart.Insert(stepIndex, "(");
                        leftPart.Add(")");
                        leftPart.Add(tokens[index]);
                        ++index;
                        continue;
                    }
                    else
                    {
                        switch (tokens[index])
                        {
                            case "+":
                                operation = Operations.Add;
                                break;
                            case "-":
                                operation = Operations.Subtract;
                                break;
                            default:
                                throw new UnacceptableExpressionException("Not defined operation");
                        }

                        rightPart = tokens.GetRange(index + 1, tokens.Count - index - 1);
                        break;
                    }
                }
                else
                {
                    if (tokens[index] == "+" || tokens[index] == "-")
                    {
                        switch (tokens[index])
                        {
                            case "+":
                                operation = Operations.Add;
                                break;
                            case "-":
                                operation = Operations.Subtract;
                                break;
                            default:
                                throw new UnacceptableExpressionException("Undefined operation");
                        }

                        rightPart = tokens.GetRange(index + 1, tokens.Count - index - 1);
                        break;
                    }

                    leftPart.Add(tokens[index]);
                    ++index;
                }
            }

            if (rightPart.Count == 0)
            {
                Boolean readyToUse = !canBeSimlified(tokens);
                while (!readyToUse) readyToUse = !canBeSimlified(tokens);
                useGrammarMult(tokens, node);
            }
            else
            {
                node.Operation = operation;
                node.LeftChild = new ASTreeNode(leftPart, Operations.NotDefined);
                node.RightChild = new ASTreeNode(rightPart, Operations.NotDefined);

                Boolean readyToUse = !canBeSimlified(leftPart);
                while (!readyToUse) readyToUse = !canBeSimlified(leftPart);
                useGrammarAdd(leftPart, node.LeftChild);

                readyToUse = !canBeSimlified(rightPart);
                while (!readyToUse) readyToUse = !canBeSimlified(rightPart);
                useGrammarAdd(rightPart, node.RightChild);
            }
        }

        private void useGrammarMult(List<String> tokens, ASTreeNode node)
        {
            var leftPart = new List<string>();
            var rightPart = new List<string>();
            UInt16 index = 0;
            Operations operation = Operations.NotDefined;

            while (index < tokens.Count)
            {
                UInt16 stepIndex = index;
                UInt16 openBrackets = 0;
                if (tokens[index] == "(")
                {
                    ++openBrackets;
                    ++index;
                    while (openBrackets > 0)
                    {
                        if (tokens[index] == "(") ++openBrackets;

                        leftPart.Add(tokens[index]);
                        ++index;
                        if (index >= tokens.Count)
                            throw new UnacceptableExpressionException("Absence of a closing bracket");
                        if (tokens[index] == ")") --openBrackets;
                    }

                    ++index;
                    if (index >= tokens.Count) break;

                    if (!(tokens[index] == "*" || tokens[index] == "/"))
                        throw new UnacceptableExpressionException("Undefined operation");

                    switch (tokens[index])
                    {
                        case "*":
                            operation = Operations.Multiply;
                            break;
                        case "/":
                            operation = Operations.Divide;
                            break;
                        default:
                            throw new UnacceptableExpressionException("Undefined operation");
                    }

                    rightPart = tokens.GetRange(index + 1, tokens.Count - index - 1);
                    break;
                }
                else
                {
                    if (tokens[index] == "*" || tokens[index] == "/")
                    {
                        switch (tokens[index])
                        {
                            case "*":
                                operation = Operations.Multiply;
                                break;
                            case "/":
                                operation = Operations.Divide;
                                break;
                            default:
                                throw new UnacceptableExpressionException("Undefined operation");
                        }

                        rightPart = tokens.GetRange(index + 1, tokens.Count - index - 1);
                        break;
                    }

                    leftPart.Add(tokens[index]);
                    ++index;
                }
            }

            if (rightPart.Count == 0)
            {
                Boolean readyToUse = !canBeSimlified(tokens);
                while (!readyToUse) readyToUse = !canBeSimlified(tokens);
                useGrammarNumber(tokens, node);
            }
            else
            {
                node.Operation = operation;
                node.LeftChild = new ASTreeNode(leftPart, Operations.NotDefined);
                node.RightChild = new ASTreeNode(rightPart, Operations.NotDefined);

                useGrammarAdd(leftPart, node.LeftChild);
                useGrammarAdd(rightPart, node.RightChild);
            }
        }

        private void useGrammarNumber(List<String> tokens, ASTreeNode node)
        {
            if (tokens.Count > 1) throw new UnacceptableExpressionException("Expected a single token");
            if (Int32.TryParse(tokens[0], out Int32 result))
            {
                node.IsLeaf = true;
                node.NumberValue = result;
                node.Operation = Operations.NotDefined;
                node.StringListValue = new List<String>() {result.ToString()};
            }
            else
            {
                node.IsLeaf = true;
                node.IsVariable = true;
                node.Operation = Operations.NotDefined;
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
            input.RemoveAt(input.Count-1);
            return true;
        }
    }
}