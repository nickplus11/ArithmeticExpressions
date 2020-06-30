using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            String exp = "((1 + Q) + 10)";
            var tokens = Parser.Parse(exp);
            var c = new ASTreeCreator(tokens);
            c.BuildASTree();
        }
    }
}
