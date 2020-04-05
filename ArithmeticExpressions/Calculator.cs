using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArithmeticExpressions
{
    public class Calculator
    {
        public Stack<string> Operations { get; private set; }
        public List<string> OutputList { get; private set; }
        private Dictionary<string, int> priority = new Dictionary<string, int>()
        {
            {"+", 1 },
            {"-", 1 },
            {"*", 2 },
            {"/", 2 }
        };

        private string GetLastOperation(string lexeme)
        {
            for (int i = lexeme.Length - 1; i >= 0; --i)
            {
                if (priority.ContainsKey(lexeme[i].ToString()))
                {
                    return lexeme[i].ToString();
                }
            }

            return null;
        }

        private double? GetLastConstant(string lexeme)
        {
            for (int i = lexeme.Length - 1; i >= 0; --i)
            {
                if (priority.ContainsKey(lexeme[i].ToString()))
                {
                    double result;
                    if (double.TryParse(lexeme.Substring(i + 1, lexeme.Length - i - 1), out result))
                    {
                        return result;
                    }
                    else return null;
                }
            }

            return null;
        }

        private void PushOperation()
        {
            OutputList.Add(Operations.Pop());
        }

        public Calculator(List<string> expression)
        {
            Operations = new Stack<string>();
            OutputList = new List<string>();

            foreach (var lexeme in expression)
            {
                if (lexeme == "(")
                {
                    Operations.Push(lexeme);
                    continue;
                }

                if (lexeme == ")")
                {
                    while (Operations.Peek() != "(")
                    {
                        PushOperation();
                    }
                    Operations.Pop();
                    continue;
                }

                if (priority.ContainsKey(lexeme))
                {
                    if (Operations.Count > 0
                        && priority.ContainsKey(Operations.Peek())
                        && priority[lexeme] < priority[Operations.Peek()])
                    {
                        PushOperation();
                    }
                    Operations.Push(lexeme);
                    continue;
                }

                OutputList.Add(lexeme);
            }

            while (Operations.Count > 0) { PushOperation(); }
        }

        public List<string> Calculate()
        {
            int index = 0;
            while (index < OutputList.Count)
            {
                if (priority.ContainsKey(OutputList[index]))
                {
                    double firstItem, secondItem;
                    var canNotBeCounted = !Double.TryParse(OutputList[index - 2], out firstItem)
                                           | !Double.TryParse(OutputList[index - 1], out secondItem);

                    if (canNotBeCounted)
                    {
                        string lastOperation = GetLastOperation(OutputList[index - 2]);
                        if (lastOperation == null || priority[lastOperation] < priority[OutputList[index]])
                        {
                            if (OutputList[index - 2].Length == 1)
                            {
                                OutputList[index] = OutputList[index - 2]
                                                    + " "
                                                    + OutputList[index]
                                                    + " "
                                                    + OutputList[index - 1];
                            }
                            else
                            {
                                OutputList[index] = "("
                                                    + OutputList[index - 2]
                                                    + ")"
                                                    + " "
                                                    + OutputList[index]
                                                    + " "
                                                    + OutputList[index - 1];
                            }
                        }
                        else
                        {
                            OutputList[index] = OutputList[index - 2]
                                                + " "
                                                + OutputList[index]
                                                + " "
                                                + OutputList[index - 1];
                        }
                    }
                    else
                    {
                        switch (OutputList[index])
                        {
                            case "+":
                                OutputList[index] = (firstItem + secondItem).ToString();
                                break;
                            case "-":
                                OutputList[index] = (firstItem - secondItem).ToString();
                                break;
                            case "*":
                                OutputList[index] = (firstItem * secondItem).ToString();
                                break;
                            case "/":
                                OutputList[index] = (firstItem / secondItem).ToString();
                                break;
                        }
                    }

                    OutputList.RemoveAt(index - 2);
                    OutputList.RemoveAt(index - 2);
                    --index;
                }
                else
                {
                    ++index;
                }
            }

            return OutputList;
        }
    }
}
