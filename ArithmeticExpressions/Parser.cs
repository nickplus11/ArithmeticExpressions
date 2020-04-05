using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpressions
{
    public static class Parser
    {
        public static List<string> Parse(string inputLexeme)
        {
            int changedIndex = 0;
            bool parsingIsFinished = false;
            var result = inputLexeme.Split(' ').ToList<string>();

            while (!parsingIsFinished)
            {
                for (var i = changedIndex; i < result.Count; ++i)
                {
                    if (result[i].Length > 1 && result[i][0] == '(')
                    {
                        string dividedPart = result[i].Substring(1);
                        result[i] = result[i].First().ToString();
                        result.Insert(i + 1, dividedPart);
                        changedIndex = i + 1;
                        break;
                    }

                    if (result[i].Length > 1 && result[i][result[i].Length - 1] == ')')
                    {
                        string dividedPart = result[i].Substring(0, result[i].Length - 1);
                        result.Insert(i + 1, result[i].Last().ToString());
                        result[i] = dividedPart;
                        changedIndex = i;
                        break;
                    }

                    if (i == result.Count - 1)
                    {
                        parsingIsFinished = true;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
