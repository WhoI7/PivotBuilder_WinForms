using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExelProb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txPath.Text = openFileDialog.FileName;
               
            }
        }
        public void CreatePivotFromExcel(string inputPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("File not found: " + inputPath);
            }

            var outputPath = Path.Combine(
                Path.GetDirectoryName(inputPath),
                Path.GetFileNameWithoutExtension(inputPath) + "_analysis.xlsx"
            );

            using (var inputWorkbook = new XLWorkbook(inputPath))
            using (var outputWorkbook = new XLWorkbook())
            {
                var inputSheet = inputWorkbook.Worksheets.First();
                var dataRange = inputSheet.RangeUsed();

                // Copy data to new workbook
                var dataSheet = outputWorkbook.Worksheets.Add("Data");
                dataRange.CopyTo(dataSheet.Cell("A1"));

                var copiedRange = dataSheet.RangeUsed();
                var pivotSheet = outputWorkbook.Worksheets.Add("Pivot");

                var pivotTable = pivotSheet.PivotTables.Add("PivotTable1", pivotSheet.Cell("A1"), copiedRange);

                // NOTE: Change column names here based on your actual table
                pivotTable.RowLabels.Add("Doctor");   // Column with names
                pivotTable.ReportFilters.Add("Status"); // E.g., Registered / Rejected
                pivotTable.Values.Add("Visits").SetSummaryFormula(XLPivotSummary.Sum); // Numeric field to summarize

                outputWorkbook.SaveAs(outputPath);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreatePivotFromExcel(txPath.Text);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
