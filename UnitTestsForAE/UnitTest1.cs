using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArithmeticExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace UnitTestsForAE
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod] 
        public void TestMethodParser1()
        {
            // Arrange
            const string inputString = "(a * (10 + 5)) / ((2 + 2) * 3) + b * (3 + 5) + 2 * 2";
            string[] expected = {
                "(","a","*","(","10","+","5",")",")","/","(","(","2","+","2",")","*","3",")",
                "+","b","*","(","3","+","5",")","+","2","*","2"
            };
            // Act
            List<string> actual = Parser.Parse(inputString);
            // Assert

            Assert.AreEqual(expected.Length, actual.Count);
            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
    }

    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void TestMethodCalculator1()
        {
            // Arrange
            const string inputString = "(a * (10 + 5)) / ((2 + 2) * 3) + b * (3 + 5) + 2 * 2";
            List<string> expected = new List<string>()
            {
                "a * 15 / 12 + b * 8 + 4"
            };
            // Act
            var calculator = new Calculator(Parser.Parse(inputString));
            List<string> actual = calculator.Calculate();
            // Assert

            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMethodCalculator2()
        {
            // Arrange
            const string inputString = "(x + (10 + 10 / 10)) * 2";
            List<string> expected = new List<string>()
            {
                "(x + 11) * 2"
            };
            // Act
            var calculator = new Calculator(Parser.Parse(inputString));
            List<string> actual = calculator.Calculate();
            // Assert

            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMethodCalculator3()
        {
            // Arrange
            const string inputString = "x - (10 + 25) / 5 + ( 1 + 2) * b - q * 2";
            List<string> expected = new List<string>()
            {
                "x - 7 + 3 * b - q * 2"
            };
            // Act
            var calculator = new Calculator(Parser.Parse(inputString));
            List<string> actual = calculator.Calculate();
            // Assert

            CollectionAssert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class WithoutConstantsTests
    {
        [TestMethod]
        public void TestMethodCalculator1()
        {
            // Arrange
            const string inputString = "(1 + 2 + 3) / 5";
            List<string> expected = new List<string>()
            {
                "1,2"
            };
            // Act
            var calculator = new Calculator(Parser.Parse(inputString));
            List<string> actual = calculator.Calculate();
            // Assert

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestMethodCalculator2()
        {
            // Arrange
            const string inputString = "(1 + 2 + 3) / 5 + 1 * 9";
            List<string> expected = new List<string>()
            {
                "10,2"
            };
            // Act
            var calculator = new Calculator(Parser.Parse(inputString));
            List<string> actual = calculator.Calculate();
            // Assert

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestMethodCalculator3()
        {
            // Arrange
            const string inputString = "((1 + 1) + (1 * 100)) / 2";
            List<string> expected = new List<string>()
            {
                "51"
            };
            // Act
            var calculator = new Calculator(Parser.Parse(inputString));
            List<string> actual = calculator.Calculate();
            // Assert

            CollectionAssert.AreEqual(expected, actual);

        }
    }
}
