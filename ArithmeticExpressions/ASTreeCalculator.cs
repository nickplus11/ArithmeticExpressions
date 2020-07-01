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
        
        public String GetSimplifiedExpressionInSingleString()
        {
            var result = Root.GetExpression();
            String formatedResult = String.Empty;

            foreach (var str in result)
            {
                formatedResult += str + " ";
            }

            formatedResult = formatedResult.Remove(formatedResult.Length-1);

            return formatedResult;
        }
    }
}