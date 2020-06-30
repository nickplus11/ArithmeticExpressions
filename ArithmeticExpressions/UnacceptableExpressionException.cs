using System;
using System.Reflection.PortableExecutable;

namespace ArithmeticExpressions
{
    public class UnacceptableExpressionException : Exception
    {
        public UnacceptableExpressionException() : this("No Message")
        {
        }

        public UnacceptableExpressionException(String message) : base(message)
        {
        }
    }
}