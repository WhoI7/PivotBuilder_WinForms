using Aspose.Cells.Pivot;
using Aspose.Cells;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestObzore
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            LoadSave();
        }

        private void LoadSave()
        {
            panelSave.Controls.Clear();
            
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Save");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return;
            }
            panelSave.Controls.Clear();

            string[] files = Directory.GetFiles(dir, "*.json");
            int y = 5;

            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                CheckBox cb = new CheckBox
                {
                    Text = fileName,
                    Location = new Point(15, y),
                    AutoSize = true
                };

                panelSave.Controls.Add(cb);
                y += cb.Height + 5;
            }


        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txPath.Text = openFileDialog.FileName;

            }
        }

        async void btNew_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }


            

            try
            {

                string path = txPath.Text.Trim();
                List<ColumnData> columns = new List<ColumnData>();

                if (!File.Exists(path))
                {
                    MessageBox.Show("Файл не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Path.GetExtension(path).ToLower() != ".xlsx")
                {
                    MessageBox.Show("Підтримуються лише файли .xlsx.", "Неправильний формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string filePath = txPath.Text;
                progressBar1.Value = 5;
                progressBar1.Visible = true;


                await Task.Run(() =>
                {
                    using (var workbook = new XLWorkbook(filePath))
                    {

                        var sourceSheet = workbook.Worksheet(1);
                        var dataRange = sourceSheet.RangeUsed();//діапазон

                        int firstRowNumber = dataRange.FirstRowUsed().RowNumber();
                        int firstColumnNumber = dataRange.FirstColumnUsed().ColumnNumber();

                        int totalRows = dataRange.RowCount();


                        int totalColumns = dataRange.ColumnCount();
                        for (int columnNumber = firstColumnNumber; columnNumber < firstColumnNumber + totalColumns; columnNumber++)
                        {

                            ColumnData dataColumn = new ColumnData();
                            List<string> columnIndividualVal = new List<string>();
                            for (int row = firstRowNumber; row < firstColumnNumber + totalRows; row++)
                            {


                                if (row == firstRowNumber)
                                {
                                    dataColumn.Name = sourceSheet.Cell(row, columnNumber).Value.ToString();
                                }
                                else
                                {
                                    string dataComirk = sourceSheet.Cell(row, columnNumber).Value.ToString();
                                    if (!columnIndividualVal.Contains(dataComirk) && dataComirk != "") columnIndividualVal.Add(dataComirk);
                                    if (columnIndividualVal.Count > 300)
                                    {
                                        columnIndividualVal = new List<string>();
                                        columnIndividualVal.Add("NULL");
                                        break;
                                    }
                                }


                            }
                            columnIndividualVal.Sort();
                            dataColumn.UniqueValues = columnIndividualVal;
                            columns.Add(dataColumn);


                            int currentProgress = (int)((columnNumber - firstColumnNumber + 1) * 100.0 / totalColumns);
                            this.Invoke(new Action(() =>
                            {
                                progressBar1.Value = currentProgress;
                            }));
                        }



                    }

                });


                TableSet tableSet = new TableSet(columns,path);
                this.Hide();
                tableSet.ShowDialog();
                this.Show();
                LoadSave();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося відкрити файл:\n" + ex.Message, "Помилка відкриття", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                }
            }
        }

        private List<ColumnData> LoadColumns(string path, IProgress<int> progress)
        {
            var columnList = new List<ColumnData>();

            using (var workbook = new XLWorkbook(path))
            {
                var worksheet = workbook.Worksheet(1);
                var range = worksheet.RangeUsed();
                int startRow = range.FirstRowUsed().RowNumber();
                int startCol = range.FirstColumnUsed().ColumnNumber();
                int totalCols = range.ColumnCount();
                int totalRows = range.RowCount();

                for (int col = startCol; col < startCol + totalCols; col++)
                {
                    ColumnData column = new ColumnData();
                    List<string> values = new List<string>();

                    for (int row = startRow; row < startRow + totalRows; row++)
                    {
                        var cell = worksheet.Cell(row, col);
                        var cellValue = cell.IsEmpty() ? "" : cell.Value.ToString();

                        if (row == startRow)
                        {
                            column.Name = cellValue;
                        }
                        else if (!string.IsNullOrEmpty(cellValue) && !values.Contains(cellValue))
                        {
                            values.Add(cellValue);
                            if (values.Count > 300)
                            {
                                values.Clear();
                                values.Add("NULL");
                                break;
                            }
                        }
                    }

                    values.Sort();
                    column.UniqueValues = values;
                    columnList.Add(column);

                    int currentProgress = (int)((col - startCol + 1) * 100.0 / totalCols);
                    progress?.Report(currentProgress);
                }
            }

            return columnList;
        }



        private string BuildPivotTables(string path, List<string> selectedSaves, List<ColumnData> columns)
        {
            string baseName = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);
            string dir = Path.GetDirectoryName(path);
            int fileCounter = 1;

            List<string> successLog = new List<string>();
            string outputPath;

            using (var workbook = new Workbook(path))
            {
                Worksheet sourceSheet = workbook.Worksheets[0];

                foreach (string saveName in selectedSaves)
                {
                    Save save = Save.LoadFromJson(saveName);
                    if (!save.CheckFiled(columns))
                    {
                        successLog.Add($"❌ - {saveName} - Відсутні необхідні поля");
                        continue;
                    }

                    string sheetName = saveName;
                    int suffix = 1;
                    while (workbook.Worksheets.Any(ws => ws.Name == sheetName))
                        sheetName = saveName + suffix++;

                    Worksheet pivotSheet = workbook.Worksheets.Add(sheetName);

                    CellArea sourceRange = new CellArea
                    {
                        StartRow = sourceSheet.Cells.MinDataRow,
                        StartColumn = sourceSheet.Cells.MinDataColumn,
                        EndRow = sourceSheet.Cells.MaxDataRow,
                        EndColumn = sourceSheet.Cells.MaxDataColumn
                    };

                    string startCell = CellsHelper.CellIndexToName(sourceRange.StartRow, sourceRange.StartColumn);
                    string endCell = CellsHelper.CellIndexToName(sourceRange.EndRow, sourceRange.EndColumn);
                    string range = $"'{sourceSheet.Name}'!{startCell}:{endCell}";

                    int pivotIndex = pivotSheet.PivotTables.Add(range, 0, 0, "Pivot_" + saveName);
                    PivotTable pt = pivotSheet.PivotTables[pivotIndex];

                    pt.RefreshData();
                    pt.CalculateData();

                    foreach (string field in save.Filters)
                        pt.AddFieldToArea(PivotFieldType.Page, field);
                    foreach (string field in save.Row)
                        pt.RowFields[pt.AddFieldToArea(PivotFieldType.Row, field)].IsAutoSort = true;
                    foreach (string field in save.Column)
                        pt.ColumnFields[pt.AddFieldToArea(PivotFieldType.Column, field)].IsAutoSort = true;
                    foreach (string field in save.Value)
                        pt.DataFields[pt.AddFieldToArea(PivotFieldType.Data, field)].Function = ConsolidationFunction.Count;

                    if (pt.DataFields.Count > 1)
                        pt.AddFieldToArea(PivotFieldType.Column, pt.DataField);

                    foreach (var filter in save.data)
                    {
                        var fields = pt.PageFields.Cast<PivotField>()
                            .Concat(pt.RowFields.Cast<PivotField>())
                            .Concat(pt.ColumnFields.Cast<PivotField>());
                        PivotField pf = fields.FirstOrDefault(f => f.Name == filter.Name);
                        if (pf != null)
                        {
                            pf.IsMultipleItemSelectionAllowed = true;
                            foreach (PivotItem item in pf.PivotItems)
                            {
                                string val = item.Value?.ToString() ?? "";
                                item.IsHidden = filter.UniqueValues.Count > 0 && !filter.UniqueValues.Contains(val);
                            }
                        }
                    }

                    pt.RefreshData();
                    pt.CalculateData();

                    successLog.Add($"✅ - {saveName}");
                }

                do
                {
                    outputPath = Path.Combine(dir, $"{baseName}_Statistic{fileCounter}{ext}");
                    fileCounter++;
                } while (File.Exists(outputPath));

                workbook.Save(outputPath);
            }

            return outputPath;
        }



        async private void btOpens_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Value = 0;

            try
            {
                foreach (Control control in this.Controls)
                    control.Enabled = false;

                List<string> selectedSaves = panelSave.Controls
                    .OfType<CheckBox>()
                    .Where(cb => cb.Checked)
                    .Select(cb => cb.Text)
                    .ToList();

                if (selectedSaves.Count == 0)
                {
                    MessageBox.Show("Виберіть хоча б одну таблицю для побудови.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string path = txPath.Text.Trim();
                if (!File.Exists(path))
                {
                    MessageBox.Show("Файл не знайдено. Вкажіть правильний шлях до Excel файлу.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Path.GetExtension(path).ToLower() != ".xlsx")
                {
                    MessageBox.Show("Підтримуються лише файли .xlsx.", "Неправильний формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var progress = new Progress<int>(value => progressBar1.Value = value);

                // 1/3 часу — Збір колонок з унікальними значеннями
                var columns = await Task.Run(() =>
                {
                    var columnList = new List<ColumnData>();

                    using (var workbook = new XLWorkbook(path))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var range = worksheet.RangeUsed();
                        int startRow = range.FirstRowUsed().RowNumber();
                        int startCol = range.FirstColumnUsed().ColumnNumber();
                        int totalCols = range.ColumnCount();
                        int totalRows = range.RowCount();

                        for (int col = startCol; col < startCol + totalCols; col++)
                        {
                            ColumnData column = new ColumnData();
                            List<string> values = new List<string>();

                            for (int row = startRow; row < startRow + totalRows; row++)
                            {
                                var cell = worksheet.Cell(row, col);
                                var cellValue = cell.IsEmpty() ? "" : cell.Value.ToString();

                                if (row == startRow)
                                {
                                    column.Name = cellValue;
                                }
                                else if (!string.IsNullOrEmpty(cellValue) && !values.Contains(cellValue))
                                {
                                    values.Add(cellValue);
                                    if (values.Count > 300)
                                    {
                                        values.Clear();
                                        values.Add("NULL");
                                        break;
                                    }
                                }
                            }

                            values.Sort();
                            column.UniqueValues = values;
                            columnList.Add(column);

                            // Прогрес для збору колонок
                            int progressVal = (int)(((col - startCol + 1) / (double)totalCols) * 33);
                            ((IProgress<int>)progress).Report(progressVal);
                        }
                    }

                    return columnList;
                });

                List<string> successLog = new List<string>();
                string outputPath = null;

                // 2/3 часу — Побудова зведених таблиць і збереження
                await Task.Run(() =>
                {
                    string baseName = Path.GetFileNameWithoutExtension(path);
                    string ext = Path.GetExtension(path);
                    string dir = Path.GetDirectoryName(path);
                    int fileCounter = 1;

                    using (var workbook = new Workbook(path))
                    {
                        Worksheet sourceSheet = workbook.Worksheets[0];

                        for (int i = 0; i < selectedSaves.Count; i++)
                        {
                            string saveName = selectedSaves[i];
                            Save save = Save.LoadFromJson(saveName);
                            if (!save.CheckFiled(columns))
                            {
                                successLog.Add($"❌ - {saveName} - Відсутні необхідні поля");
                                continue;
                            }

                            string sheetName = saveName;
                            int suffix = 1;
                            bool nameExists = true;

                            while (nameExists)
                            {
                                nameExists = false;
                                for (int j = 0; j < workbook.Worksheets.Count; j++)
                                {
                                    if (workbook.Worksheets[j].Name == sheetName)
                                    {
                                        nameExists = true;
                                        sheetName = saveName + suffix;
                                        suffix++;
                                        break;
                                    }
                                }
                            }

                            Worksheet pivotSheet = workbook.Worksheets.Add(sheetName);

                            CellArea sourceRange = new CellArea
                            {
                                StartRow = sourceSheet.Cells.MinDataRow,
                                StartColumn = sourceSheet.Cells.MinDataColumn,
                                EndRow = sourceSheet.Cells.MaxDataRow,
                                EndColumn = sourceSheet.Cells.MaxDataColumn
                            };

                            string startCell = CellsHelper.CellIndexToName(sourceRange.StartRow, sourceRange.StartColumn);
                            string endCell = CellsHelper.CellIndexToName(sourceRange.EndRow, sourceRange.EndColumn);
                            string range = $"'{sourceSheet.Name}'!{startCell}:{endCell}";

                            int pivotIndex = pivotSheet.PivotTables.Add(range, 0, 0, "Pivot_" + saveName);
                            PivotTable pivotTable = pivotSheet.PivotTables[pivotIndex];

                            pivotTable.RefreshData();
                            pivotTable.CalculateData();

                            foreach (string field in save.Filters)
                                pivotTable.AddFieldToArea(PivotFieldType.Page, field);

                            foreach (string field in save.Row)
                            {
                                int idx = pivotTable.AddFieldToArea(PivotFieldType.Row, field);
                                PivotField pf = pivotTable.RowFields[idx];
                                pf.IsAutoSort = true;
                            }

                            foreach (string field in save.Column)
                            {
                                int idx = pivotTable.AddFieldToArea(PivotFieldType.Column, field);
                                PivotField pf = pivotTable.ColumnFields[idx];
                                pf.IsAutoSort = true;
                            }

                            foreach (string field in save.Value)
                            {
                                int idx = pivotTable.AddFieldToArea(PivotFieldType.Data, field);
                                pivotTable.DataFields[idx].Function = ConsolidationFunction.Count;
                            }

                            if (pivotTable.DataFields.Count > 1)
                            {
                                pivotTable.AddFieldToArea(PivotFieldType.Column, pivotTable.DataField);
                            }

                            foreach (var filter in save.data)
                            {
                                PivotField pf = null;

                                for (int f = 0; f < pivotTable.PageFields.Count; f++)
                                {
                                    if (pivotTable.PageFields[f].Name == filter.Name)
                                    {
                                        pf = pivotTable.PageFields[f];
                                        break;
                                    }
                                }

                                if (pf == null)
                                {
                                    for (int f = 0; f < pivotTable.RowFields.Count; f++)
                                    {
                                        if (pivotTable.RowFields[f].Name == filter.Name)
                                        {
                                            pf = pivotTable.RowFields[f];
                                            break;
                                        }
                                    }
                                }

                                if (pf == null)
                                {
                                    for (int f = 0; f < pivotTable.ColumnFields.Count; f++)
                                    {
                                        if (pivotTable.ColumnFields[f].Name == filter.Name)
                                        {
                                            pf = pivotTable.ColumnFields[f];
                                            break;
                                        }
                                    }
                                }

                                if (pf != null)
                                {
                                    pf.IsMultipleItemSelectionAllowed = true;

                                    foreach (PivotItem item in pf.PivotItems)
                                    {
                                        string val = item.Value?.ToString() ?? "";
                                        item.IsHidden = filter.UniqueValues.Count > 0 && !filter.UniqueValues.Contains(val);
                                    }
                                }
                            }

                            pivotTable.RefreshData();
                            pivotTable.CalculateData();

                            successLog.Add($"✅ - {saveName}");

                            // Прогрес для побудови таблиць
                            int progressVal = 33 + (int)(((i + 1) / (double)selectedSaves.Count) * 66);
                            ((IProgress<int>)progress).Report(progressVal);
                        }

                        // Генерація унікального шляху для збереження
                        do
                        {
                            outputPath = Path.Combine(dir, $"{baseName}_Statistic{fileCounter}{ext}");
                            fileCounter++;
                        } while (File.Exists(outputPath));

                        workbook.Save(outputPath);
                    }
                });

                // 100% — очистка "Evaluation Warning" і остаточний прогрес
                

                // Показати повідомлення з результатом
                string message = successLog.Count > 0
                    ? "Були побудовані такі таблиці:\n" + string.Join("\n", successLog)
                    : "Не було побудовано жодної таблиці через відсутність необхідних полів.";

                message += $"\n\nФайл збережено: {outputPath}";

                MessageBox.Show(message, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося відкрити файл:\n" + ex.Message, "Помилка відкриття", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
                foreach (Control control in this.Controls)
                    control.Enabled = true;
            }
        }




        private void btDelet_Click(object sender, EventArgs e)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Save");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return;
            }

            var selected = panelSave.Controls
                .OfType<CheckBox>()
                .Where(cb => cb.Checked)
                .Select(cb => cb.Text.Trim())
                .ToList();

            if (selected.Count == 0)
                return;

            // Формуємо повідомлення зі списком назв
            string fileList = string.Join(Environment.NewLine, selected);
            string message = "Видалити вибрані збереження?" + Environment.NewLine + Environment.NewLine + fileList;

            var result = MessageBox.Show(
                message,
                "Підтвердження видалення",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.OK)
                return;

            foreach (var name in selected)
            {
                string fullPath = Path.Combine(dir, name + ".json");
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch
                    {

                        // Опціонально: лог або повідомлення про помилку
                    }
                }
            }


            LoadSave();
        }
    }
}
