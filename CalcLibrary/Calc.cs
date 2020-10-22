using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CalcLibrary
{
    public class Calc
    {
        private delegate T OperationDelegate<T>(T x, T y);
        private delegate T FunctionDelegate<T>(T x);

        private static string[] GetOperands(string s)
        {
            string pattern = @"(?:[^\d\.,\-eπ]|(?<=\d)-)+";
            Regex rgx = new Regex(pattern);
            return rgx.Split(s);
        }

        private static string GetOperation(string s)
        {
            string pattern = @"(?:[^\d\.,\-eπ]|(?<=\d)-)+";
            Regex rgx = new Regex(pattern);
            return rgx.Match(s).ToString();
        }

        private static readonly Dictionary<string, OperationDelegate<double>> DoubleOperation =
            new Dictionary<string, OperationDelegate<double>>()
            {
                {"+", (x, y) => x + y},
                {"-", (x, y) => x - y},
                {"*", (x, y) => x * y},
                {"/", (x, y) => x / y},
                {"div", (x, y) => (int)x / (int)y},
                {"mod", (x, y) => (int)x % (int)y},
                {"^", (x, y) => Math.Pow(x,y)},
            };

        private static readonly Dictionary<string, FunctionDelegate<double>> DoubleFunction =
            new Dictionary<string, FunctionDelegate<double>>()
            {
                {"sin", Math.Sin},
                {"cos", Math.Cos},
                {"tan", Math.Tan},
                {"e^", Math.Exp},
                {"√", Math.Sqrt},
                {"n!", Factorial},
            };

        public static string DoOperation(string s)
        {
            string[] operands = GetOperands(s);
            string op = GetOperation(s);
            for (int i = 0; i < operands.Length; i++)
            {
                if (operands[i] == "π")
                    operands[i] = Math.PI.ToString();
                else if (operands[i] == "-π")
                    operands[i] = (-Math.PI).ToString();
                else if (operands[i] == "e")
                    operands[i] = Math.E.ToString();
                else if (operands[i] == "-e")
                    operands[i] = (-Math.E).ToString();
            }
            string res = "";
            if (operands.Length == 0)
                res = "";
            else if (op == "")
                res = operands[0];
            else if (DoubleOperation.ContainsKey(op))
                res = DoubleOperation[op](double.Parse(operands[0]), double.Parse(operands[1])).ToString();
            else
                res = DoubleFunction[op](double.Parse(operands[0])).ToString();
            return res;
        }

        private static double Factorial(double n)
        {
            if (Math.Round(n) != n)
                throw new ArithmeticException();
            double res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }
    }
}
