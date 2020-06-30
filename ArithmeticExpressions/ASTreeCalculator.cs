using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    public class ASTreeCalculator
    {
        public ASTreeNode Root { get; private set; }
        
        public ASTreeCalculator(ASTreeNode rootNode)
        {
            Root = rootNode;
        }

        public List<String> GetSimplifiedExpression()
        {
            return Root.GetExpression();
        }
    }
}