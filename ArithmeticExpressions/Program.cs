using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            //String exp = "(a * (10 + 5)) / ((2 + 2) * 3) + b * (3 + 5) + 2 * 2";
            String exp = "(x + (10 + 10 / 10)) * 2";
            var tokens = Parser.Parse(exp);
            var cr = new ASTreeCreator(tokens);
            cr.BuildASTree();
            var calc = new ASTreeCalculator(cr.Root);
            var tmp = calc.GetSimplifiedExpression();
            foreach (var elem in calc.GetSimplifiedExpression())
            {
                Console.Write(elem, " ");
            }
        }
    }
}
