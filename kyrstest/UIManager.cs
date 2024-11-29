using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PeterO.Numbers;

public static class UIManager
{
    public static void ShowInfo(string message)
    {
        MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void DisplayResults(List<EDecimal[]> results, TextBox resultBox, EDecimal stepSize)
    {
        EDecimal t = EDecimal.Zero; // Время инициализируется с 0
        resultBox.Clear(); // Очищаем текстовое поле перед выводом

        // Проходим по результатам
        foreach (var result in results)
        {
            string output = $"t: {t.ToString()}";
            for (int i = 0; i < result.Length; i++)
            {
                // Проверяем значение на NaN и выводим "NaN" явно
                string value = result[i].IsNaN() ? "NaN" : result[i].ToString();
                output += $", y{i + 1}: {value}";
            }
            resultBox.AppendText(output + Environment.NewLine);
            t = t.Add(stepSize); // Увеличиваем время на шаг
        }
    }


    public static void ShowError(string errorMessage)
    {
        MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void DisplaySystemInfo(string systemInfo, TextBox infoBox)
    {
        infoBox.Clear();
        infoBox.AppendText(systemInfo);
    }
}
