using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            //String expression = "ENTER_THE_EXPRESSION";
            String expression = "(x + (10 + 10 / 10)) * 2";
            var tokens = Parser.Parse(expression);
            var asTreeCreator = new ASTreeCreator(tokens);
            asTreeCreator.BuildASTree();
            var asTreeCalculator = new ASTreeCalculator(asTreeCreator.Root);
            var result = asTreeCalculator.GetSimplifiedExpression();
            foreach (var elem in result)
            {
                Console.Write("{0} ", elem);
            }
        }
    }
}
