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
            string[] expected =
            {
                "(", "a", "*", "(", "10", "+", "5", ")", ")", "/", "(", "(", "2", "+", "2", ")", "*", "3", ")",
                "+", "b", "*", "(", "3", "+", "5", ")", "+", "2", "*", "2"
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
            const String inputString = "(a * (10 + 5)) / ((2 + 2) * 3) + b * (3 + 5) + 2 * 2";
            var expected = "a * 15 / 12 + b * 8 + 4";

            // Act
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpressionInSingleString();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodCalculator2()
        {
            // Arrange
            const string inputString = "(x + (10 + 10 / 10)) * 2";
            var expected = "( x + 11 ) * 2";

            // Act
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpressionInSingleString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodCalculator3()
        {
            // Arrange
            const string inputString = "x - (10 + 25) / 5 + ( 1 + 2) * b - q * 2";
            var expected = "x - 7 + 3 * b - q * 2";

            // Act
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpressionInSingleString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void TestMethodCalculator4()
        {
            // Arrange
            const String inputString = "Y * 10 * 2 / 4 * ( 1 + 2 + ( 3 + 3 ) + 1 ) - X";
            var expected = "Y * 50 - X";

            // Act
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpressionInSingleString();

            //Assert
            Assert.AreEqual(expected, actual);
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
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpression();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodCalculator2()
        {
            // Arrange
            const string inputString = "(1 + 2 + 3) / 5 + ( 1 * 9 )";
            List<string> expected = new List<string>()
            {
                "10,2"
            };

            // Act
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpression();

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
            var cr = new ASTreeCreator(Parser.Parse(inputString));
            cr.BuildASTree();
            var root = cr.Root;
            var calculator = new ASTreeCalculator(root);
            var actual = calculator.GetSimplifiedExpression();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}