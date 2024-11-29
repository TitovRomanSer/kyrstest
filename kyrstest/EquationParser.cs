using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PeterO.Numbers;

namespace DifferentialEquationSolver
{
    public class EquationParser
    {
        // Парсинг начальных условий
        public (EDecimal[] initialConditions, EDecimal[] initialDerivatives) ParseInitialConditions(string input)
        {
            var matches = Regex.Matches(input, @"y\d+\(0\)\s*=\s*-?\d+(\.\d+)?([eE][+-]?\d+)?|y\d+'\(0\)\s*=\s*-?\d+(\.\d+)?([eE][+-]?\d+)?");

            if (matches.Count == 0)
            {
                throw new FormatException("Некорректный формат начальных условий. Убедитесь, что начальные условия введены в формате y1(0)=значение или y1'(0)=значение.");
            }

            var initialConditions = new List<EDecimal>();
            var initialDerivatives = new List<EDecimal>();

            foreach (Match match in matches)
            {
                string valuePart = Regex.Replace(match.Value, @"y\d+'?\(0\)\s*=", "").Trim();
                try
                {
                    var value = EDecimal.FromString(valuePart);
                    if (match.Value.Contains("'"))
                    {
                        initialDerivatives.Add(value);
                    }
                    else
                    {
                        initialConditions.Add(value);
                    }
                }
                catch (FormatException)
                {
                    throw new FormatException($"Ошибка парсинга значения: {valuePart}. Убедитесь, что формат корректен.");
                }
            }

            Console.WriteLine("Обработанные начальные условия: " + string.Join(", ", initialConditions.Select(v => v.ToString())));
            Console.WriteLine("Обработанные начальные производные: " + string.Join(", ", initialDerivatives.Select(v => v.ToString())));

            return (initialConditions.ToArray(), initialDerivatives.ToArray());
        }

        // Парсинг уравнений
        public List<Func<EDecimal, EDecimal[], EDecimal>> ParseEquations(string input)
        {
            var equations = new List<Func<EDecimal, EDecimal[], EDecimal>>();
            var equationStrings = input.Split(';').Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e));

            foreach (var equationString in equationStrings)
            {
                var cleanedEquation = CleanEquation(equationString);
                equations.Add(CreateEquationFunction(cleanedEquation));
            }

            return equations;
        }

        private string CleanEquation(string equation)
        {
            equation = Regex.Replace(equation, @"y(\d+)''", "SecDeriv(y$1)");
            equation = Regex.Replace(equation, @"y(\d+)'", "Deriv(y$1)");
            equation = equation.Replace(" ", "");
            Console.WriteLine($"Обработанное уравнение: {equation}");
            return equation;
        }

        private Func<EDecimal, EDecimal[], EDecimal> CreateEquationFunction(string equation)
        {
            var parts = equation.Split('=');
            if (parts.Length != 2)
            {
                throw new FormatException($"Некорректный формат уравнения: {equation}");
            }

            var rightSide = parts[1];
            return (t, y) =>
            {
                var evaluator = new MathEvaluator();
                evaluator.SetVariable("t", t);
                for (int i = 0; i < y.Length; i++)
                {
                    evaluator.SetVariable($"y{i + 1}", y[i]);
                }

                try
                {
                    var result = evaluator.Evaluate(rightSide);
                    Console.WriteLine($"Вычисление правой части '{rightSide}' при t={t}, y={string.Join(", ", y.Select(v => v.ToString()))}: {result}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при вычислении правой части '{rightSide}': {ex.Message}");
                    throw;
                }
            };
        }
    }

    // Класс для вычисления математических выражений
    public class MathEvaluator
    {
        private readonly Dictionary<string, EDecimal> _variables = new Dictionary<string, EDecimal>();

        public MathEvaluator()
        {
            // Добавляем основные константы
            _variables["e"] = EDecimal.FromDouble(Math.E);
            _variables["pi"] = EDecimal.FromDouble(Math.PI);
        }
        public void SetVariable(string name, EDecimal value)
        {
            _variables[name] = value;
        }

        public EDecimal Evaluate(string expression)
        {
            try
            {
                expression = expression.Replace(" ", "");

                // Математические операции
                if (expression.Contains("+"))
                {
                    var parts = SplitExpression(expression, '+');
                    return Evaluate(parts[0]).Add(Evaluate(parts[1]));
                }
                if (expression.Contains("-"))
                {
                    var parts = SplitExpression(expression, '-');
                    return Evaluate(parts[0]).Subtract(Evaluate(parts[1]));
                }
                if (expression.Contains("*"))
                {
                    var parts = SplitExpression(expression, '*');
                    return Evaluate(parts[0]).Multiply(Evaluate(parts[1]));
                }
                if (expression.Contains("/"))
                {
                    var parts = SplitExpression(expression, '/');
                    var denominator = Evaluate(parts[1]);
                    if (denominator.IsZero)
                        throw new DivideByZeroException("Деление на ноль.");
                    return Evaluate(parts[0]).Divide(denominator, EContext.ForPrecision(50));
                }
                if (expression.Contains("^"))
                {
                    var parts = SplitExpression(expression, '^');
                    return Evaluate(parts[0]).Pow(Evaluate(parts[1]).ToInt32Checked());
                }

                // Тригонометрические функции
                if (expression.StartsWith("sin"))
                    return CalculateSin(Evaluate(ExtractArgument(expression, "sin")));
                if (expression.StartsWith("cos"))
                    return CalculateCos(Evaluate(ExtractArgument(expression, "cos")));
                if (expression.StartsWith("tan"))
                    return CalculateTan(Evaluate(ExtractArgument(expression, "tan")));
                if (expression.StartsWith("cot"))
                    return CalculateCot(Evaluate(ExtractArgument(expression, "cot")));

                // Логарифмы и корни
                if (expression.StartsWith("sqrt"))
                    return CalculateSqrt(Evaluate(ExtractArgument(expression, "sqrt")));
                if (expression.StartsWith("ln"))
                    return CalculateLn(Evaluate(ExtractArgument(expression, "ln")));

                // Экспонента
                if (expression.StartsWith("exp"))
                    return CalculateExp(Evaluate(ExtractArgument(expression, "exp")));

                // Если переменная
                if (_variables.ContainsKey(expression))
                    return _variables[expression];

                // Если число
                return EDecimal.FromString(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка вычисления выражения '{expression}': {ex.Message}");
            }
        }

        // Математические функции
        private EDecimal CalculateSin(EDecimal x) => EDecimal.FromDouble(Math.Sin(x.ToDouble()));
        private EDecimal CalculateCos(EDecimal x) => EDecimal.FromDouble(Math.Cos(x.ToDouble()));
        private EDecimal CalculateTan(EDecimal x) => CalculateSin(x).Divide(CalculateCos(x), EContext.ForPrecision(50));
        private EDecimal CalculateCot(EDecimal x) => CalculateCos(x).Divide(CalculateSin(x), EContext.ForPrecision(50));
        private EDecimal CalculateSqrt(EDecimal x) => x.Sqrt(EContext.ForPrecision(50));
        private EDecimal CalculateLn(EDecimal x) => EDecimal.FromDouble(Math.Log(x.ToDouble()));
        private EDecimal CalculateExp(EDecimal x) => EDecimal.FromDouble(Math.Exp(x.ToDouble()));

        // Вспомогательные методы
        private string[] SplitExpression(string expression, char op)
        {
            var parts = new List<string>();
            int depth = 0, start = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(') depth++;
                if (expression[i] == ')') depth--;
                if (expression[i] == op && depth == 0)
                {
                    parts.Add(expression.Substring(start, i - start));
                    start = i + 1;
                }
            }

            parts.Add(expression.Substring(start));
            return parts.ToArray();
        }

        private string ExtractArgument(string expression, string functionName)
        {
            int start = expression.IndexOf(functionName) + functionName.Length + 1;
            int end = expression.IndexOf(')', start);
            return expression.Substring(start, end - start);
        }
    }
}
