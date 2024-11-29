using System;
using System.Collections.Generic;
using System.Linq;
using PeterO.Numbers;

namespace DifferentialEquationSolver
{
    public class EquationSystem
    {
        public List<Func<EDecimal, EDecimal[], EDecimal>> Equations { get; set; }
        public EDecimal[] InitialConditions { get; set; }
        public EDecimal[] InitialDerivatives { get; set; }
        public EDecimal StepSize { get; set; }
        public EDecimal EndTime { get; set; }
        public int Order { get; set; }
        public int VariableCount { get; set; }

        public MainForm MainForm
        {
            get => default;
            set
            {
            }
        }

        public EquationSystem(
            List<Func<EDecimal, EDecimal[], EDecimal>> equations,
            EDecimal[] initialConditions,
            EDecimal stepSize,
            EDecimal endTime,
            int order,
            int variableCount,
            EDecimal[] initialDerivatives = null
        )
        {
            if (equations == null || equations.Count != variableCount)
                throw new ArgumentException("Количество уравнений должно соответствовать количеству переменных.");
            if (initialConditions == null || initialConditions.Length != variableCount)
                throw new ArgumentException("Количество начальных условий должно совпадать с количеством переменных.");
            if (stepSize.CompareTo(EDecimal.Zero) <= 0)
                throw new ArgumentException("Шаг интегрирования должен быть положительным.");
            if (endTime.CompareTo(EDecimal.Zero) <= 0)
                throw new ArgumentException("Время окончания должно быть положительным.");

            Equations = equations;
            InitialConditions = initialConditions;
            StepSize = stepSize;
            EndTime = endTime;
            Order = order;
            VariableCount = variableCount;
            InitialDerivatives = initialDerivatives;
        }

        public string GetInitialConditionsString()
        {
            return string.Join(", ", InitialConditions.Select(ic => ic.ToString()));
        }

        public string GetSystemInfo()
        {
            return $"Порядок: {Order}, Переменных: {VariableCount}, Шаг: {StepSize}, Время окончания: {EndTime}";
        }
    }
}
