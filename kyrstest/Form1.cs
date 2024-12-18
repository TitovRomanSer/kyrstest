using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using PeterO.Numbers;

namespace DifferentialEquationSolver
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            stepSizeBox.LostFocus += (sender, e) => ValidateStepSize();
            initialConditionsBox.LostFocus += (sender, e) => ValidateInitialConditions();
             
            equationsBox.LostFocus += (sender, e) => ValidateEquations();
            endTimeBox.LostFocus += (sender, e) => ValidateEndTime();
        }
        private void ValidateStepSize()
        {
            if (string.IsNullOrWhiteSpace(stepSizeBox.Text)) return; // Пропускаем проверку, если поле пустое

            if (!TryParseEDecimal(stepSizeBox.Text.Replace(",", "."), out EDecimal stepSize) || stepSize.CompareTo(EDecimal.Zero) <= 0)
            {
                UIManager.ShowError("Шаг интегрирования должен быть положительным числом.");
                stepSizeBox.Focus(); // Вернуть фокус в поле с ошибкой
            }
        }
        private Random random = new Random();

        private void GenerateRandomEquationsButton_Click(object sender, EventArgs e)
        {
            try
            {
                int numberOfEquations = 10000; // Количество уравнений и начальных условий
                Random random = new Random();

                // Генерация начальных условий и уравнений
                var initialConditions = new List<string>();
                var equations = new List<string>();

                for (int i = 1; i <= numberOfEquations; i++)
                {
                    // Случайное целое число для начальных условий
                    int initialValue = random.Next(-1000, 1000);
                    string formattedInitialValue = initialValue < 0 ? $"({initialValue})" : $"{initialValue}";
                    initialConditions.Add($"y{i}(0)={formattedInitialValue}");

                    // Случайное целое число для уравнений
                    int equationValue = random.Next(-1000, 1000);
                    string formattedEquationValue = equationValue < 0 ? $"({equationValue})" : $"{equationValue}";
                    equations.Add($"y'{i}={formattedEquationValue}");
                }

                // Объединение начальных условий и уравнений
                string initialConditionsString = string.Join(", ", initialConditions);
                string equationsString = string.Join("; ", equations);

                // Вывод начальных условий и уравнений на форму
                initialConditionsBox.Text = initialConditionsString;
                equationsBox.Text = equationsString;

                MessageBox.Show("Случайные уравнения и начальные условия успешно сгенерированы!", "Генерация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                UIManager.ShowError("Произошла ошибка при генерации уравнений: " + ex.Message);
            }
        }







        // Метод проверки корректности начальных условий
        private void ValidateInitialConditions()
        {
            if (string.IsNullOrWhiteSpace(initialConditionsBox.Text)) return; // Пропускаем проверку, если поле пустое

            try
            {
                var parser = new EquationParser();
                parser.ParseInitialConditions(initialConditionsBox.Text); // Проверка парсинга
            }
            catch (Exception ex)
            {
                UIManager.ShowError("Начальные условия некорректны: " + ex.Message);
                initialConditionsBox.Focus(); // Вернуть фокус в поле с ошибкой
            }
        }

        // Метод проверки корректности системы уравнений
        private void ValidateEquations()
        {
            if (string.IsNullOrWhiteSpace(equationsBox.Text)) return; // Пропускаем проверку, если поле пустое

            try
            {
                var parser = new EquationParser();
                parser.ParseEquations(equationsBox.Text); // Проверка парсинга
            }
            catch (Exception ex)
            {
                UIManager.ShowError("Система уравнений некорректна: " + ex.Message);
                equationsBox.Focus(); // Вернуть фокус в поле с ошибкой
            }
        }

        // Метод проверки корректности времени окончания
        private void ValidateEndTime()
        {
            if (string.IsNullOrWhiteSpace(endTimeBox.Text)) return; // Пропускаем проверку, если поле пустое

            if (int.TryParse(endTimeBox.Text, out int endTime))
            {
                if (endTime > 100)
                {
                    UIManager.ShowError("Время окончания не может быть больше 100.");
                    endTimeBox.Focus(); // Вернуть фокус в поле с ошибкой
                }
            }
            else
            {
                UIManager.ShowError("Пожалуйста, введите корректное число для времени окончания.");
                endTimeBox.Focus(); // Вернуть фокус в поле с ошибкой
            }
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на заполнение всех полей
                if (//string.IsNullOrWhiteSpace(stepSizeBox.Text) ||
                    //string.IsNullOrWhiteSpace(endTimeBox.Text) ||
                    //string.IsNullOrWhiteSpace(initialConditionsBox.Text) ||
                   string.IsNullOrWhiteSpace(equationsBox.Text) ||
                    (!firstOrderRadioButton.Checked && !secondOrderRadioButton.Checked))
                {
                    UIManager.ShowError("Все поля должны быть заполнены для выполнения расчёта.");
                    return;
                }

                // Проверка на корректность шага интегрирования
                if (!TryParseEDecimal(stepSizeBox.Text.Replace(",", "."), out EDecimal stepSize) || stepSize.CompareTo(EDecimal.Zero) <= 0)
                {
                    UIManager.ShowError("Шаг интегрирования должен быть положительным числом.");
                    return;
                }

                // Проверка на корректность конечного времени
                if (!TryParseEDecimal(endTimeBox.Text, out EDecimal endTime) || endTime.CompareTo(EDecimal.Zero) <= 0)
                {
                    UIManager.ShowError("Конечное время должно быть положительным числом.");
                    return;
                }
                // Добавление проверки на максимальное значение времени окончания (не более 1000)
                if (endTime.CompareTo(EDecimal.FromInt32(1000)) > 0)
                {
                    UIManager.ShowError("Конечное время не должно превышать 1000.");
                    return;
                }
                // Парсинг системы уравнений
                var parser = new EquationParser();
                List<Func<EDecimal, EDecimal[], EDecimal>> equations;
                try
                {
                    equations = parser.ParseEquations(equationsBox.Text);

                }
                catch
                {
                    UIManager.ShowError("Некорректный формат системы уравнений. Проверьте ввод.");
                    return;
                }

                // Парсинг начальных условий
                var (initialConditions, initialDerivatives) = parser.ParseInitialConditions(initialConditionsBox.Text);

                // Определение порядка системы уравнений
                int order = firstOrderRadioButton.Checked ? 1 : 2;

                // Создание объекта системы уравнений
                EquationSystem system = new EquationSystem(
                    equations,
                    initialConditions,
                    stepSize,
                    endTime,
                    order,
                    initialConditions.Length
                );

                // Решение системы уравнений методом Рунге-Кутты
                RungeKuttaSolver solver = new RungeKuttaSolver();
                List<EDecimal[]> results;
                if (order == 1)
                {
                    results = solver.SolveFirstOrder(system);
                }
                else
                {
                    if (initialDerivatives == null || initialDerivatives.Length == 0)
                    {
                        UIManager.ShowError("Для решения уравнений второго порядка необходимо указать начальные производные.");
                        return;
                    }
                    results = solver.SolveSecondOrder(system, initialDerivatives);
                }

                // Вывод результатов в текстовое поле
                UIManager.DisplayResults(results, resultBox, stepSize);
            }
            catch (Exception ex)
            {
                UIManager.ShowError("Произошла ошибка при выполнении расчёта: " + ex.Message);
            }
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files|*.txt"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Получаем параметры из интерфейса
                    if (!TryParseEDecimal(stepSizeBox.Text, out EDecimal stepSize))
                    {
                        UIManager.ShowError("Некорректное значение шага интегрирования.");
                        return;
                    }
                    if (!TryParseEDecimal(endTimeBox.Text, out EDecimal endTime))
                    {
                        UIManager.ShowError("Некорректное значение конечного времени.");
                        return;
                    }

                    var parser = new EquationParser();
                    var (initialConditions, initialDerivatives) = parser.ParseInitialConditions(initialConditionsBox.Text);
                    string[] equations = equationsBox.Text.Split(';');
                    int order = firstOrderRadioButton.Checked ? 1 : 2;

                    // Решение системы уравнений для получения результатов
                    List<Func<EDecimal, EDecimal[], EDecimal>> parsedEquations = parser.ParseEquations(equationsBox.Text);
                    EquationSystem system = new EquationSystem(parsedEquations, initialConditions, stepSize, endTime, order, initialConditions.Length);

                    RungeKuttaSolver solver = new RungeKuttaSolver();
                    List<EDecimal[]> results;
                    if (order == 1)
                    {
                        results = solver.SolveFirstOrder(system);
                    }
                    else
                    {
                        results = solver.SolveSecondOrder(system, initialDerivatives);
                    }

                    // Сохранение в файл
                    DataManager.SaveAllDataToFile(stepSize, endTime, initialConditions, equations, order, results, saveFileDialog.FileName);
                    MessageBox.Show("Данные успешно сохранены!", "Сохранение файла", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    UIManager.ShowError("Ошибка при сохранении файла: " + ex.Message);
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files|*.txt"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
                    if (lines.Length >= 4)
                    {
                        stepSizeBox.Text = lines[0];
                        endTimeBox.Text = lines[1];
                        initialConditionsBox.Text = lines[2];
                        equationsBox.Text = lines[3];
                        MessageBox.Show("Данные из файла успешно загружены!", "Загрузка файла", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UIManager.ShowError("Файл содержит недостаточное количество данных. Ожидаются шаг, конечное время, начальные условия и система уравнений.");
                    }
                }
                catch (Exception ex)
                {
                    UIManager.ShowError("Ошибка при чтении файла: " + ex.Message);
                }
            }
        }
        // Метод TryParse для EDecimal
        private bool TryParseEDecimal(string input, out EDecimal result)
        {
            try
            {
                result = EDecimal.FromString(input.Trim());
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        private void resultBox_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(resultBox.Text))
            {
                UIManager.ShowError("Нет данных для печати. Сначала выполните расчёты.");
                return;
            }

            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDialog.Document = printDocument;
            printDocument.PrintPage += PrintDocument_PrintPage;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    UIManager.ShowError("Ошибка при печати: " + ex.Message);
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string printText = resultBox.Text;
            try
            {
                e.Graphics.DrawString(printText, new Font("Arial", 14), Brushes.Black, 100, 100);
            }
            catch (Exception ex)
            {
                UIManager.ShowError("Ошибка при выводе текста на принтер: " + ex.Message);
            }
        }
    }
}
