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
            Int32 index = tokens.Count - 1;
            Operations operation = Operations.NotDefined;

            while (index >= 0)
            {
                UInt16 closedBrackets = 0;
                if (tokens[index] == ")")
                {
                    ++closedBrackets;
                    rightPart.Add(tokens[index]);
                    --index;

                    while (closedBrackets > 0)
                    {
                        if (index < 0) throw new UnacceptableExpressionException("Not closed brackets");
                        if (tokens[index] == "(") --closedBrackets;
                        if (tokens[index] == ")") ++closedBrackets;

                        rightPart.Add(tokens[index]);
                        --index;
                    }

                    continue;
                }

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

                    leftPart = tokens.GetRange(0, index);
                    break;
                }

                rightPart.Add(tokens[index]);
                --index;
            }

            if (leftPart.Count == 0)
            {
                Boolean readyToUse = !canBeSimlified(tokens);
                while (!readyToUse) readyToUse = !canBeSimlified(tokens);
                useGrammarMult(tokens, node);
            }
            else
            {
                //leftPart.Reverse();
                rightPart.Reverse();

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
            Int32 index = tokens.Count - 1;
            Operations operation = Operations.NotDefined;

            while (index >= 0)
            {
                UInt16 closedBrackets = 0;
                if (tokens[index] == ")")
                {
                    ++closedBrackets;
                    rightPart.Add(tokens[index]);
                    --index;

                    while (closedBrackets > 0)
                    {
                        if (index < 0) throw new UnacceptableExpressionException("Not closed brackets");
                        if (tokens[index] == "(") --closedBrackets;
                        if (tokens[index] == ")") ++closedBrackets;

                        rightPart.Add(tokens[index]);
                        --index;
                    }

                    // --index;
                    // if (index < 0) break;

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

                    leftPart = tokens.GetRange(0, index);
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

                        leftPart = tokens.GetRange(0, index);
                        break;
                    }

                    rightPart.Add(tokens[index]);
                    --index;
                }
            }

            if (leftPart.Count == 0)
            {
                Boolean readyToUse = !canBeSimlified(tokens);
                while (!readyToUse) readyToUse = !canBeSimlified(tokens);
                useGrammarNumber(tokens, node);
            }
            else
            {
                //leftPart.Reverse();
                rightPart.Reverse();
                
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

        private void useGrammarNumber(List<String> tokens, ASTreeNode node)
        {
            if (tokens.Count > 1) throw new UnacceptableExpressionException("Expected a single token");
            if (Double.TryParse(tokens[0], out Double result))
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
            input.RemoveAt(input.Count - 1);
            return true;
        }
    }
}