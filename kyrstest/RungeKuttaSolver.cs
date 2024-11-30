using System;
using System.Collections.Generic;
using System.Linq;
using PeterO.Numbers;

namespace DifferentialEquationSolver
{
    public class RungeKuttaSolver
    {
        private const int MaxAdjustmentAttempts = 10;
        private static readonly EDecimal MinStep = EDecimal.FromString("0.00001");
        private static readonly EDecimal MaxStep = EDecimal.FromString("100");
        // Решение ДУ первого порядка
        public List<EDecimal[]> SolveFirstOrder(EquationSystem system)
        {
            EDecimal t = EDecimal.Zero;
            EDecimal[] y = system.InitialConditions.ToArray();
            EDecimal step = system.StepSize;
            EDecimal endTime = system.EndTime;
            List<EDecimal[]> results = new List<EDecimal[]>();

            int adjustmentAttempts = 0;

            while (adjustmentAttempts < MaxAdjustmentAttempts)
            {
                try
                {
                    results.Clear(); // Очищаем результаты при каждой попытке
                    t = EDecimal.Zero; // Сбрасываем время
                    y = system.InitialConditions.ToArray(); // Сбрасываем начальные условия

                    while (t.CompareTo(endTime) <= 0)
                    {
                        results.Add(y.Select(RoundToFourDecimals).ToArray());
                        Log($"Шаг t={RoundToFourDecimals(t)}, y={string.Join(", ", y.Select(RoundToFourDecimals))}");

                        // Проверка на некорректные значения y
                        ValidateValues(y, t);

                        // Выполняем шаг Рунге-Кутты для первого порядка
                        y = RungeKuttaStepFirstOrder(system.Equations, t, y, step);

                        t = t.Add(step);
                    }

                    // Если выполнение завершилось без ошибок, выходим из цикла
                    break;
                }
                catch (Exception ex)
                {
                    adjustmentAttempts++;
                    AdjustStepSize(ref step, ex.Message);
                }
            }

            if (adjustmentAttempts >= MaxAdjustmentAttempts)
            {
                throw new InvalidOperationException("Не удалось подобрать подходящий шаг интегрирования. Попробуйте указать другой шаг.");
            }

            return results;
        }

        // Решение ДУ второго порядка
        public List<EDecimal[]> SolveSecondOrder(EquationSystem system, EDecimal[] initialDerivatives)
        {
            EDecimal t = EDecimal.Zero;
            EDecimal step = system.StepSize;
            EDecimal endTime = system.EndTime;
            int variableCount = system.VariableCount;

            EDecimal[] y = system.InitialConditions.ToArray();
            EDecimal[] yPrime = initialDerivatives.ToArray();

            List<EDecimal[]> results = new List<EDecimal[]>();
            int adjustmentAttempts = 0;

            while (adjustmentAttempts < MaxAdjustmentAttempts)
            {
                try
                {
                    results.Clear();
                    t = EDecimal.Zero;
                    y = system.InitialConditions.ToArray();
                    yPrime = initialDerivatives.ToArray();

                    while (t.CompareTo(endTime) <= 0)
                    {
                        // Сохраняем текущие значения y и y' для записи в результаты
                        EDecimal[] currentState = new EDecimal[variableCount * 2];
                        for (int i = 0; i < variableCount; i++)
                        {
                            currentState[i] = RoundToFourDecimals(y[i]);
                            currentState[variableCount + i] = RoundToFourDecimals(yPrime[i]);
                        }
                        results.Add(currentState);

                        // Проверка на некорректные значения y и y'
                        ValidateValues(y, t);
                        ValidateValues(yPrime, t);

                        // Выполняем шаг Рунге-Кутты для второго порядка
                        y = RungeKuttaStepSecondOrder(system.Equations, t, y, yPrime, step);
                        yPrime = UpdateDerivatives(system.Equations, t, y, yPrime, step);

                        t = t.Add(step);
                    }

                    // Если выполнение завершилось без ошибок, выходим из цикла
                    break;
                }
                catch (Exception ex)
                {
                    adjustmentAttempts++;
                    AdjustStepSize(ref step, ex.Message);
                }
            }

            if (adjustmentAttempts >= MaxAdjustmentAttempts)
            {
                throw new InvalidOperationException("Не удалось подобрать подходящий шаг интегрирования. Попробуйте указать другой шаг.");
            }

            return results;
        }

        // Регулировка шага
        private void AdjustStepSize(ref EDecimal step, string errorMessage)
        {
            bool stepAdjusted = false;

            if (errorMessage.Contains("NaN") || errorMessage.Contains("Infinity"))
            {
                // Уменьшаем шаг, если значение некорректное
                if (step.CompareTo(EDecimal.FromInt32(3)) > 0)
                {
                    step = step.Divide(2); // Уменьшаем шаг на 50%, если он больше 3
                    Log($"Шаг уменьшен на 50% до {step} для предотвращения ошибок.");
                }
                else
                {
                    step = step.Subtract(EDecimal.FromString("0.1")); // Уменьшаем шаг на 0.1, если он ≤ 3
                    Log($"Шаг уменьшен на 0.1 до {step} для предотвращения ошибок.");
                }
                stepAdjusted = true;
            }
            else
            {
                // Увеличиваем шаг, если значение слишком маленькое
                if (step.CompareTo(EDecimal.FromString("0.0001")) <= 0)
                {
                    step = step.Add(EDecimal.FromString("0.1")); // Увеличиваем шаг на 0.1, если он ≤ 3
                    Log($"Шаг увеличен на 0.1 до {step} для улучшения вычислений.");
                }
                else
                {
                    step = step.Multiply(EDecimal.FromString("0.1")); // Увеличиваем шаг на 0.1, если он > 3
                    Log($"Шаг увеличен на 0.1 до {step} для улучшения вычислений.");
                }
                stepAdjusted = true;
            }

            // Ограничиваем шаг в пределах допустимых значений
            if (step.CompareTo(MinStep) < 0)
            {
                step = MinStep;
                Log($"Шаг ограничен минимальным значением: {MinStep}");
                stepAdjusted = true;
            }
            else if (step.CompareTo(MaxStep) > 0)
            {
                step = MaxStep;
                Log($"Шаг ограничен максимальным значением: {MaxStep}");
                stepAdjusted = true;
            }

            // Уведомляем пользователя о корректировке шага
            if (stepAdjusted)
            {
                UIManager.ShowInfo($"Шаг интегрирования был скорректирован до {step} для обеспечения корректного выполнения расчёта.");
            }
        }



        // Шаг метода Рунге-Кутты для первого порядка
        private EDecimal[] RungeKuttaStepFirstOrder(List<Func<EDecimal, EDecimal[], EDecimal>> equations, EDecimal t, EDecimal[] y, EDecimal h)
        {
            int n = y.Length;
            EDecimal[] k1 = CalculateK(equations, t, y);
            EDecimal[] k2 = CalculateK(equations, t.Add(h.Divide(2)), AddVectors(y, MultiplyVector(h.Divide(2), k1)));
            EDecimal[] k3 = CalculateK(equations, t.Add(h.Divide(2)), AddVectors(y, MultiplyVector(h.Divide(2), k2)));
            EDecimal[] k4 = CalculateK(equations, t.Add(h), AddVectors(y, MultiplyVector(h, k3)));

            // Проверка корректности коэффициентов
            ValidateValues(k1, t);
            ValidateValues(k2, t);
            ValidateValues(k3, t);
            ValidateValues(k4, t);

            EDecimal[] result = new EDecimal[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = y[i].Add(h.Divide(6).Multiply(k1[i].Add(k2[i].Multiply(2)).Add(k3[i].Multiply(2)).Add(k4[i])));
            }

            return result.Select(RoundToFourDecimals).ToArray();
        }

        // Шаг метода Рунге-Кутты для второго порядка
        private EDecimal[] RungeKuttaStepSecondOrder(List<Func<EDecimal, EDecimal[], EDecimal>> equations, EDecimal t, EDecimal[] y, EDecimal[] yPrime, EDecimal h)
        {
            EDecimal[] k1y = yPrime;
            EDecimal[] k1yPrime = CalculateK(equations, t, y);

            EDecimal[] yMid1 = AddVectors(y, MultiplyVector(h.Divide(2), k1y));
            EDecimal[] yPrimeMid1 = AddVectors(yPrime, MultiplyVector(h.Divide(2), k1yPrime));
            EDecimal[] k2y = yPrimeMid1;
            EDecimal[] k2yPrime = CalculateK(equations, t.Add(h.Divide(2)), yMid1);

            EDecimal[] yMid2 = AddVectors(y, MultiplyVector(h.Divide(2), k2y));
            EDecimal[] yPrimeMid2 = AddVectors(yPrime, MultiplyVector(h.Divide(2), k2yPrime));
            EDecimal[] k3y = yPrimeMid2;
            EDecimal[] k3yPrime = CalculateK(equations, t.Add(h.Divide(2)), yMid2);

            EDecimal[] yEnd = AddVectors(y, MultiplyVector(h, k3y));
            EDecimal[] yPrimeEnd = AddVectors(yPrime, MultiplyVector(h, k3yPrime));
            EDecimal[] k4y = yPrimeEnd;
            EDecimal[] k4yPrime = CalculateK(equations, t.Add(h), yEnd);

            EDecimal[] result = new EDecimal[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                result[i] = y[i].Add(h.Divide(6).Multiply(k1y[i].Add(k2y[i].Multiply(2)).Add(k3y[i].Multiply(2)).Add(k4y[i])));
            }

            return result.Select(RoundToFourDecimals).ToArray();
        }

        // Обновление производных для второго порядка
        private EDecimal[] UpdateDerivatives(List<Func<EDecimal, EDecimal[], EDecimal>> equations, EDecimal t, EDecimal[] y, EDecimal[] yPrime, EDecimal h)
        {
            int n = y.Length;
            EDecimal[] k1 = CalculateK(equations, t, y);
            EDecimal[] k2 = CalculateK(equations, t.Add(h.Divide(2)), AddVectors(y, MultiplyVector(h.Divide(2), k1)));
            EDecimal[] k3 = CalculateK(equations, t.Add(h.Divide(2)), AddVectors(y, MultiplyVector(h.Divide(2), k2)));
            EDecimal[] k4 = CalculateK(equations, t.Add(h), AddVectors(y, MultiplyVector(h, k3)));

            // Проверка корректности производных
            ValidateValues(k1, t);
            ValidateValues(k2, t);
            ValidateValues(k3, t);
            ValidateValues(k4, t);

            EDecimal[] updatedDerivatives = new EDecimal[n];
            for (int i = 0; i < n; i++)
            {
                updatedDerivatives[i] = yPrime[i].Add(h.Divide(6).Multiply(k1[i].Add(k2[i].Multiply(2)).Add(k3[i].Multiply(2)).Add(k4[i])));
            }

            return updatedDerivatives.Select(RoundToFourDecimals).ToArray();
        }

        // Вычисление коэффициентов k
        private EDecimal[] CalculateK(List<Func<EDecimal, EDecimal[], EDecimal>> equations, EDecimal t, EDecimal[] y)
        {
            int n = equations.Count;
            EDecimal[] k = new EDecimal[n];
            for (int i = 0; i < n; i++)
            {
                k[i] = equations[i](t, y);
            }
            return k;
        }

        // Проверка корректности значений
        private void ValidateValues(EDecimal[] values, EDecimal t)
        {
            foreach (var value in values)
            {
                if (value.IsNaN() || value.IsInfinity())
                {
                    throw new Exception($"Некорректное значение обнаружено на шаге t={t}: {value}");
                }
            }
        }

        // Вспомогательные методы
        private EDecimal[] AddVectors(EDecimal[] v1, EDecimal[] v2)
        {
            EDecimal[] result = new EDecimal[v1.Length];
            for (int i = 0; i < v1.Length; i++) result[i] = v1[i].Add(v2[i]);
            return result;
        }

        private EDecimal[] MultiplyVector(EDecimal scalar, EDecimal[] v)
        {
            EDecimal[] result = new EDecimal[v.Length];
            for (int i = 0; i < v.Length; i++) result[i] = scalar.Multiply(v[i]);
            return result;
        }

        private EDecimal RoundToFourDecimals(EDecimal value)
        {
            return value.RoundToExponent(-6, EContext.ForPrecision(50));
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
