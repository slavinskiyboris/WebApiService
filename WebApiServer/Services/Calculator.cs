using System.Globalization;
using System.Text.RegularExpressions;
using CommonLibrary;

namespace WebApiServer.Services
{
    public static class Calculator
    {
        public static double EvaluateExpression(string expression)
        {
            expression = expression.Replace(",", "."); // Замена запятых на точки

            if (!ExpressionValidator.IsValidExpression(expression))
            {
                throw new ArgumentException("Invalid expression format.");
            }

            var tokens = Regex.Split(expression, @"([*\/+\-()])").Where(t => t.Trim() != "").Select(t => t.Trim()).ToArray();
            if (tokens.Length == 0)
            {
                throw new ArgumentException("Expression is empty.");
            }

            var values = new Stack<double>();
            var operators = new Stack<string>();

            try
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (double.TryParse(tokens[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        values.Push(value);
                    }
                    else if (tokens[i] == "(")
                    {
                        operators.Push(tokens[i]);
                    }
                    else if (tokens[i] == ")")
                    {
                        while (operators.Peek() != "(")
                        {
                            values.Push(ApplyOperator(operators.Pop(), values.Pop(), values.Pop()));
                        }
                        operators.Pop();
                    }
                    else if (IsOperator(tokens[i]))
                    {
                        if (i == 0 || i == tokens.Length - 1)
                        {
                            throw new ArgumentException("Expression cannot start or end with an operator.");
                        }

                        while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(tokens[i]))
                        {
                            values.Push(ApplyOperator(operators.Pop(), values.Pop(), values.Pop()));
                        }
                        operators.Push(tokens[i]);
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid token: {tokens[i]}");
                    }
                }

                while (operators.Count > 0)
                {
                    values.Push(ApplyOperator(operators.Pop(), values.Pop(), values.Pop()));
                }

                if (values.Count != 1)
                {
                    throw new ArgumentException("The number of operators does not match the number of operands.");
                }

                return values.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Invalid expression format.");
            }
        }

        private static int Precedence(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "(":
                case ")":
                    return 0;
                default:
                    throw new ArgumentException($"Invalid operator {op}");
            }
        }

        private static double ApplyOperator(string op, double b, double a)
        {
            switch (op)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0)
                    {
                        throw new ArgumentException("Division by zero is not allowed.");
                    }
                    return a / b;
                default:
                    throw new ArgumentException($"Invalid operator {op}");
            }
        }

        private static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }
    }
}
