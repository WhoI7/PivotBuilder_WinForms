using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestObzore;

public class Save
{
    public List<string> Filters { get; set; } = new List<string>();
    public List<string> Column { get; set; } = new List<string>();
    public List<string> Row { get; set; } = new List<string>();
    public List<string> Value { get; set; } = new List<string>();
    public List<ColumnData> data { get; set; } = new List<ColumnData>();

    public void ShowAllData()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("📌 Filters:");
        foreach (var item in Filters)
            sb.AppendLine("  - " + item);

        sb.AppendLine("📌 Column:");
        foreach (var item in Column)
            sb.AppendLine("  - " + item);

        sb.AppendLine("📌 Row:");
        foreach (var item in Row)
            sb.AppendLine("  - " + item);

        sb.AppendLine("📌 Value:");
        foreach (var item in Value)
            sb.AppendLine("  - " + item);

        sb.AppendLine("📌 Data:");
        foreach (var col in data)
        {
            sb.AppendLine($"  - Name: {col.Name}");
            if (col.UniqueValues != null && col.UniqueValues.Count > 0)
            {
                sb.AppendLine("    UniqueValues:");
                foreach (var val in col.UniqueValues)
                    sb.AppendLine("      • " + val);
            }
            else
            {
                sb.AppendLine("    (немає унікальних значень)");
            }
        }

        //MessageBox.Show(sb.ToString(), "Вся інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void SaveToJson(string inputName = null)
    {
        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Save");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // Отримати ім’я з текстового поля, якщо передано
        string baseName = string.IsNullOrWhiteSpace(inputName) ? "MySave" : inputName.Trim();
        string fileName = baseName + ".json";
        string fullPath = Path.Combine(dir, fileName);

        int counter = 1;
        while (File.Exists(fullPath))
        {
            fileName = $"{baseName}{counter}.json";
            fullPath = Path.Combine(dir, fileName);
            counter++;
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(fullPath, JsonSerializer.Serialize(this, options));

        // Повідомлення про успішне збереження
        MessageBox.Show($"Збереження виконано успішно:\n{fileName}", "✅ Збережено", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static Save LoadFromJson(string inputName = null)
    {
        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Save");
        if (!Directory.Exists(dir))
            throw new DirectoryNotFoundException("Папка 'Save' не знайдена.");

        string baseName = string.IsNullOrWhiteSpace(inputName) ? "MySave" : inputName.Trim();
        string fullPath = Path.Combine(dir, baseName + ".json");

        if (!File.Exists(fullPath))
        {
            // Можна взяти перший доступний
            var files = Directory.GetFiles(dir, "*.json");
            if (files.Length == 0)
                throw new FileNotFoundException("У папці 'Save' немає файлів.");

            fullPath = files[0]; // Перший знайдений
        }

        string json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<Save>(json);
    }
    public bool CheckFiled(List<ColumnData> tableColumns)
    {
        // Створимо словник: назва поля -> список можливих значень з таблиці
        var tableDict = tableColumns
            .GroupBy(c => c.Name)
            .ToDictionary(g => g.Key, g => g.SelectMany(c => c.UniqueValues).ToHashSet());

        // Об'єднуємо всі назви полів з сейву
        var allFields = Filters.Concat(Column).Concat(Row).Concat(Value).Distinct();

        foreach (var fieldName in allFields)
        {
            // Має бути знайдена назва поля
            if (!tableDict.ContainsKey(fieldName))
                return false;

            // Знайдемо відповідний об'єкт у Save.data (у ньому очікувані значення)
            var expected = data.FirstOrDefault(d => d.Name == fieldName);
            if (expected != null)
            {
                foreach (var val in expected.UniqueValues)
                {
                    if (!tableDict[fieldName].Contains(val))
                        return false;
                }
            }
        }

        return true;
    }




}