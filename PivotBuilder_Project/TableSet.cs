using Aspose.Cells.Pivot;
using ClosedXML.Excel;
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TestObzore
{
    public partial class TableSet : Form
    {
        string path;
        public TableSet(List<ColumnData> Data,string path)
        {
            this.path = path;
            InitializeComponent();
            LoadData(Data);
        }


        private void checkBoxFCR(object sender, EventArgs e)
        {
            
            CheckBox chek = sender as CheckBox;
            chek.Checked = !chek.Checked;

            string[] element = chek.Tag.ToString().Split(new string[] { "||||" }, StringSplitOptions.None);

            if (chek.Checked)
            {
                chek.Checked = false;
            }
            else 
            {
                
                List<CheckBox> listCheckiv = new List<CheckBox>();

                foreach (Control ctrl in panelFileds.Controls)
                {
                    if (ctrl.Tag is string tagValue)
                    {
                        if (tagValue.Contains(element[1]))
                        {
                            listCheckiv.Add(ctrl as CheckBox);
                        }
                    }

                }

                listCheckiv[0].Checked = false;
                listCheckiv[1].Checked = false;
                listCheckiv[2].Checked = false;

                chek.Checked = !chek.Checked;
            }


            EnableDisable(element,chek.Checked);
        }

        private void EnableDisable(string[] FiledAndName, bool chek)
        {
            foreach (Control colums in panelColumns.Controls)
            {
                if (colums.Tag is string tagValue && (FiledAndName[0] == "Filter" || FiledAndName[0] == "Column" || FiledAndName[0] == "Row"))
                {
                    if (tagValue.Contains(FiledAndName[1]) && tagValue.Length == FiledAndName[1].Length)
                    {
                        FlowLayoutPanel panel =  colums as FlowLayoutPanel;
                        panel.Enabled = chek;

                    }
                }

            }

            if (chek)
            {
                listFilter.Items.Remove(FiledAndName[1]);
                listColumn.Items.Remove(FiledAndName[1]);
                listRow.Items.Remove(FiledAndName[1]);
                switch (FiledAndName[0])
                {
                    case "Filter": listFilter.Items.Add(FiledAndName[1]) ; break;
                    case "Column": listColumn.Items.Add(FiledAndName[1]); break;
                    case "Row": listRow.Items.Add(FiledAndName[1]); break;

                }
            }
            else
            {
                switch (FiledAndName[0])
                {
                    case "Filter": listFilter.Items.Remove(FiledAndName[1]); break;
                    case "Column": listColumn.Items.Remove(FiledAndName[1]); break;
                    case "Row": listRow.Items.Remove(FiledAndName[1]); break;

                }
            }

        }
        private void checkBoxV(object sender, EventArgs e)
        {
            CheckBox chek = sender as CheckBox;

            string[] FiledAndName = chek.Tag.ToString().Split(new string[] { "||||" }, StringSplitOptions.None);

            if (chek.Checked)
            {
                listValue.Items.Add(FiledAndName[1]);
            }
            else
            {
                listValue.Items.Remove(FiledAndName[1]);
            }
        }

        private void LoadData(List<ColumnData> columnList)
        {
            int panelFiledY = 0;
            while (columnList.Count > 0)
            {
                var column = columnList[0];
                columnList.RemoveAt(0);

                /////////////////////////////////////
                /////////// Панель вибору полів
                ////////////////////////////////////////////////////////
                CheckBox check1 = new CheckBox();
                check1.Text = "";
                check1.Tag = $"Filter||||{column.Name}";
                check1.AutoSize = true;
                check1.Click += checkBoxFCR;
                check1.Location = new Point(30, panelFiledY + 10);
                if (column.UniqueValues[0] == "NULL" && column.UniqueValues.Count == 1) check1.Visible = false;
                ////

                CheckBox check2 = new CheckBox();
                check2.Text = "";
                check2.Tag = $"Column||||{column.Name}";
                check2.AutoSize = true;
                check2.Click += checkBoxFCR;
                check2.Location = new Point(90, panelFiledY + 10);
                if (column.UniqueValues[0] == "NULL" && column.UniqueValues.Count == 1)  check2.Visible = false;
                
                ////
                CheckBox check3 = new CheckBox();
                check3.Text = "";
                check3.Tag = $"Row||||{column.Name}";
                check3.AutoSize = true;
                check3.Click += checkBoxFCR;
                check3.Location = new Point(150, panelFiledY + 10);
                if (column.UniqueValues[0] == "NULL" && column.UniqueValues.Count == 1) check3.Visible = false;

                ////
                CheckBox check4 = new CheckBox();
                check4.Text = "";
                check4.Tag = $"Value||||{column.Name}";
                check4.AutoSize = true;
                check4.Click += checkBoxV; 
                check4.Location = new Point(215, panelFiledY + 10);

                ////



                ////////////////Назва поля
                Label nameFiled = new Label();
                nameFiled.Text = column.Name;
                nameFiled.AutoSize = true;
                nameFiled.Location = new Point(280, panelFiledY + 10);
                ////



                ///////////////Лінія
                Panel line = new Panel();
                line.Size = new Size(430, 1);
                line.BorderStyle = BorderStyle.FixedSingle;
                line.Location = new Point(15, panelFiledY + 30);

                ////

                panelFileds.Controls.Add(check1);
                panelFileds.Controls.Add(check2);
                panelFileds.Controls.Add(check3);
                panelFileds.Controls.Add(check4);

                panelFileds.Controls.Add(nameFiled);

                panelFileds.Controls.Add(line);

                ///////////////////////////////////////////////////////
                /////////////Панель Стовпців по елементам
                ///////////////////////////////////////////////////////

                FlowLayoutPanel colonka = new FlowLayoutPanel();
                colonka.FlowDirection = FlowDirection.TopDown;
                colonka.BorderStyle = BorderStyle.FixedSingle;
                colonka.Tag = column.Name;
                colonka.Size = new Size(230, 270);
                colonka.WrapContents = false;
                colonka.Enabled = false;

                Label namecolonka = new Label();
                namecolonka.Text = column.Name;
                namecolonka.AutoSize = true;

                colonka.Controls.Add(namecolonka);

                CheckedListBox checkedListBox = new CheckedListBox();
                checkedListBox.Items.Add("Виділити все");
                checkedListBox.ItemCheck += checkList;
                foreach (var item in column.UniqueValues) checkedListBox.Items.Add(item);
                checkedListBox.Size = new Size(220, 240 - namecolonka.Height);

                
                colonka.Controls.Add(checkedListBox);

                panelColumns.Controls.Add(colonka);


                panelFiledY += 30;
            }
        }

        private void checkList(object sender, ItemCheckEventArgs e)
        {

            CheckedListBox clb = (CheckedListBox)sender;

            clb.ItemCheck -= checkList;

            if (e.Index == 0)
            {
                CheckState newState = e.NewValue;

                for (int i = 1; i < clb.Items.Count; i++)
                {
                    clb.SetItemCheckState(i, newState);
                }
            }
            else
            {
                if (e.NewValue == CheckState.Unchecked)
                {
                    clb.SetItemCheckState(0, CheckState.Unchecked);
                }
                else
                {
                    bool allChecked = true;
                    for (int i = 1; i < clb.Items.Count; i++)
                    {
                        if (i == e.Index)
                        {
                            if (e.NewValue != CheckState.Checked)
                            {
                                allChecked = false;
                                break;
                            }
                        }
                        else if (clb.GetItemCheckState(i) != CheckState.Checked)
                        {
                            allChecked = false;
                            break;
                        }
                    }

                    if (allChecked)
                    {
                        clb.SetItemCheckState(0, CheckState.Checked);
                    }
                }
            }

            clb.ItemCheck += checkList;

        }

        private void Buld_Table(object sender, EventArgs e)
        {
            Save save = GenerateSave();

            BuildPivotTableFromSave(save,path);

        }
        async public void BuildPivotTableFromSave(Save save, string excelPath)
        {
            if (!progressBar.Visible)
            {
                try
                {
                    progressBar.Value = 5;
                    progressBar.Visible = true;

                    foreach (Control control in this.Controls)
                    {
                         control.Enabled = false;
                    }
                    await Task.Run(() =>
                    {
                        using (var workbook = new Workbook(excelPath))
                        {

                            Worksheet sourceSheet = workbook.Worksheets[0];

                            string baseSheetName = string.IsNullOrWhiteSpace(textBox.Text) ? "PivotTable" : textBox.Text.Trim();
                            string sheetName = baseSheetName;
                            int counter = 1;

                            while (workbook.Worksheets.Any(ws => ws.Name == sheetName))
                            {
                                sheetName = baseSheetName + counter;
                                counter++;
                            }

                            progressBar.Value = 30;
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

                            progressBar.Value = 50;
                            int pivotIndex = pivotSheet.PivotTables.Add(range, 0, 0, "MyPivot");
                            PivotTable pivotTable = pivotSheet.PivotTables[pivotIndex];

                            pivotTable.RefreshData();
                            pivotTable.CalculateData();

                            // Add filters
                            foreach (string field in save.Filters)
                            {
                                pivotTable.AddFieldToArea(PivotFieldType.Page, field);
                            }

                            // Add rows with sorting
                            foreach (string field in save.Row)
                            {
                                int index = pivotTable.AddFieldToArea(PivotFieldType.Row, field);

                                PivotField pf = pivotTable.RowFields[pivotTable.RowFields.Count - 1];
                                pf.IsAutoSort = true;
                                //pf.AutoSortField = 0; // сортувати по першому підполю
                                //pf.IsAscendSort = true; // сортування за зростанням (A-Z)
                            }

                            // Add columns with sorting
                            foreach (string field in save.Column)
                            {
                                int index = pivotTable.AddFieldToArea(PivotFieldType.Column, field);

                                PivotField pf = pivotTable.ColumnFields[pivotTable.ColumnFields.Count - 1];
                                pf.IsAutoSort = true;
                                //pf.AutoSortField = 0;
                                //pf.IsAscendSort = true;
                            }


                            foreach (string field in save.Value)
                            {
                                int index = pivotTable.AddFieldToArea(PivotFieldType.Data, field);
                                pivotTable.DataFields[index].Function = ConsolidationFunction.Count;
                            }

                            // Якщо більше одного значення — додаємо "Values" у колонку
                            if (pivotTable.DataFields.Count > 1)
                            {
                                pivotTable.AddFieldToArea(PivotFieldType.Column, pivotTable.DataField);
                            }


                            progressBar.Value = 70;
                            // Apply filters
                            foreach (var filter in save.data)
                            {
                                PivotField pivotField = null;

                                foreach (PivotField field in pivotTable.PageFields)
                                {
                                    if (field.Name == filter.Name)
                                    {
                                        pivotField = field;
                                        break;
                                    }
                                }

                                if (pivotField == null)
                                {
                                    foreach (PivotField field in pivotTable.RowFields)
                                    {
                                        if (field.Name == filter.Name)
                                        {
                                            pivotField = field;
                                            break;
                                        }
                                    }
                                }

                                if (pivotField == null)
                                {
                                    foreach (PivotField field in pivotTable.ColumnFields)
                                    {
                                        if (field.Name == filter.Name)
                                        {
                                            pivotField = field;
                                            break;
                                        }
                                    }
                                }

                                if (pivotField != null)
                                {
                                    pivotField.IsMultipleItemSelectionAllowed = true;

                                    foreach (PivotItem item in pivotField.PivotItems)
                                    {
                                        string val = item.Value?.ToString() ?? "";
                                        item.IsHidden = filter.UniqueValues?.Count > 0
                                            ? !filter.UniqueValues.Contains(val)
                                            : false;
                                    }
                                }
                            }

                            progressBar.Value = 90;

                            pivotTable.RefreshData();
                            pivotTable.CalculateData();



                            string directory = Path.GetDirectoryName(excelPath);
                            string baseName = Path.GetFileNameWithoutExtension(excelPath);
                            string extension = Path.GetExtension(excelPath);
                            string newExcelPath = excelPath;

                            int fileCounter = 1;
                            while (File.Exists(newExcelPath))
                            {
                                newExcelPath = Path.Combine(directory, $"{baseName}_Statistic{fileCounter}{extension}");
                                fileCounter++;
                            }

                            // Зберігаємо файл
                            workbook.Save(newExcelPath);
                           
                            progressBar.Value = 100;
                        }
                    });
                    foreach (Control control in this.Controls)
                    {
                        control.Enabled = true;
                    }
                    progressBar.Visible = false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally { progressBar.Visible = false; }
            }
            
        }
        private Save GenerateSave()
        {
            Save save = new Save();

            foreach (string item in listFilter.Items)
            {
                ColumnData data = new ColumnData();
                save.Filters.Add(item);

                foreach (Control colums in panelColumns.Controls)
                {
                    if (colums.Tag is string tagVale)
                    {
                        if (tagVale == item)
                        {

                            data.Name = item;
                            foreach (Control control in colums.Controls)
                            {
                                if (control is CheckedListBox list)
                                {
                                    for (int i = 0; i < list.Items.Count; i++)
                                    {
                                        if (list.GetItemChecked(i))
                                        {
                                            data.UniqueValues.Add(list.Items[i].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                save.data.Add(data);
            }

            foreach (string item in listColumn.Items)
            {
                ColumnData data = new ColumnData();
                save.Column.Add(item);

                foreach (Control colums in panelColumns.Controls)
                {
                    if (colums.Tag is string tagVale)
                    {
                        if (tagVale == item)
                        {

                            data.Name = item;
                            foreach (Control control in colums.Controls)
                            {
                                if (control is CheckedListBox list)
                                {
                                    for (int i = 0; i < list.Items.Count; i++)
                                    {
                                        if (list.GetItemChecked(i))
                                        {
                                            data.UniqueValues.Add(list.Items[i].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                save.data.Add(data);
            }

            foreach (string item in listRow.Items)
            {
                ColumnData data = new ColumnData();
                save.Row.Add(item);

                foreach (Control colums in panelColumns.Controls)
                {
                    if (colums.Tag is string tagVale)
                    {
                        if (tagVale == item)
                        {

                            data.Name = item;
                            foreach (Control control in colums.Controls)
                            {
                                if (control is CheckedListBox list)
                                {
                                    for (int i = 0; i < list.Items.Count; i++)
                                    {
                                        if (list.GetItemChecked(i))
                                        {
                                            data.UniqueValues.Add(list.Items[i].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                save.data.Add(data);
            }

            foreach (string item in listValue.Items)
            {
                
                save.Value.Add(item);

            }
            
            save.ShowAllData();


            return save;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save save = GenerateSave();
            save.SaveToJson(textBox.Text);

        }
    }
}
