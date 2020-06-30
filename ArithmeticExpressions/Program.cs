using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            String exp = "(a * (10 + 5)) / ((2 + 2) * 3) + b * (3 + 5) + 2 * 2";
            //String exp = "(12 + 10 * X) - 15 + 10 * 2 / 4 + ( 1 + 2 ) + Y";
            var tokens = Parser.Parse(exp);
            var cr = new ASTreeCreator(tokens);
            cr.BuildASTree();
            var calc = new ASTreeCalculator(cr.Root);
            foreach (var elem in calc.GetSimplifiedExpression())
            {
                Console.Write(elem, " ");
            }
        }
    }
}
