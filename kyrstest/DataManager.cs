using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PeterO.Numbers;

namespace DifferentialEquationSolver
{
    public static class DataManager
    {
        // Сохранение всех данных в файл
        public static void SaveAllDataToFile(EDecimal stepSize, EDecimal endTime, EDecimal[] initialConditions, string[] equations, int order, List<EDecimal[]> results, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Сохранение шага интегрирования
                writer.WriteLine("Шаг интегрирования:");
                writer.WriteLine(stepSize.ToString());
                writer.WriteLine();

                // Сохранение конечного времени
                writer.WriteLine("Конечное время:");
                writer.WriteLine(endTime.ToString());
                writer.WriteLine();

                // Сохранение начальных условий
                writer.WriteLine("Начальные условия:");
                writer.WriteLine(string.Join(", ", initialConditions.Select(ic => ic.ToString())));
                writer.WriteLine();

                // Сохранение системы уравнений
                writer.WriteLine("Система уравнений:");
                writer.WriteLine(string.Join("; ", equations));
                writer.WriteLine();

                // Сохранение порядка системы
                writer.WriteLine("Порядок системы:");
                writer.WriteLine(order.ToString());
                writer.WriteLine();

                // Сохранение результатов
                writer.WriteLine("Результаты вычислений:");
                EDecimal t = EDecimal.Zero; // Начальное время
                foreach (var result in results)
                {
                    string resultLine = $"t: {t.ToString()}";
                    for (int i = 0; i < result.Length; i++)
                    {
                        resultLine += $", y{i + 1}: {result[i].ToString()}";
                    }
                    writer.WriteLine(resultLine);
                    t = t.Add(stepSize);
                }
            }
        }

        // Загрузка данных из файла
        public static (EDecimal stepSize, EDecimal endTime, EDecimal[] initialConditions, string[] equations, int order) LoadDataFromFile(string path)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                if (lines.Length < 9)
                {
                    throw new InvalidDataException("Файл содержит недостаточное количество данных.");
                }

                // Чтение шага интегрирования
                EDecimal stepSize = EDecimal.FromString(lines[1]); 

                // Чтение конечного времени
                EDecimal endTime = EDecimal.FromString(lines[3]);

                // Чтение начальных условий
                EDecimal[] initialConditions = lines[5].Split(',').Select(value => EDecimal.FromString(value.Trim())).ToArray();

                // Чтение системы уравнений
                string[] equations = lines[7].Split(';');

                // Чтение порядка системы
                int order = int.Parse(lines[9]);

                return (stepSize, endTime, initialConditions, equations, order);
            }
            catch (Exception ex)
            {
                throw new IOException("Ошибка при загрузке данных из файла: " + ex.Message);
            }
        }
    }
}
