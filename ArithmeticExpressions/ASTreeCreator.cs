using System;
using System.Collections.Generic;

namespace ArithmeticExpressions
{
    public class ASTreeCreator
    {
        public List<String> InputString { get; private set; }

        public ASTreeNode Root { get; private set; }

        public ASTreeCreator() : this(new List<String>())
        {
        }

        public ASTreeCreator(List<String> inputStringList)
        {
            InputString = inputStringList;
        }
        
        
        
        
    }
}