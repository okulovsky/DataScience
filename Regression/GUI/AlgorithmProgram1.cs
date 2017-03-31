using DataScience;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataScience.Regression
{
  public partial class AlgorithmProgram1
    {
        AbstractAlgorithm task;

        Form form;
        public Chart[,] Charts { get; private set; }
        
        void UpdateCharts(List<Series> series, int chartNumber)
        {
            var row = chartNumber / Charts.GetLength(0);
            var column = chartNumber % Charts.GetLength(0);

            Charts[row,column].Series.Clear();
            foreach (var s in series)
                Charts[row,column].Series.Add(s);

            Charts[row, column].Legends.Clear();
            Charts[row, column].Legends.Add(new Legend());
        }



        public  void Run(AbstractAlgorithm  _task, int rows=1, int columns=1, string name="")
        {
            task = _task;

            var height = 100.0f / rows;
            var width = 100.0f / columns;
            var table = new TableLayoutPanel();
            table.ColumnStyles.Clear();
            table.RowCount = 2;
            table.ColumnCount = 2;

            table.RowStyles.Clear();
            for (int i = 0; i < rows; i++)
                table.RowStyles.Add(new RowStyle { SizeType = SizeType.Percent, Height = height });

            table.ColumnStyles.Clear();
            for (int i = 0; i < columns; i++)
                table.ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = width });

            Charts = new Chart[rows, columns];
            for (int row=0;row<rows;row++)
                for (int column=0;column<columns;column++)
                {
                    Charts[row, column] = new Chart();
                    Charts[row, column].ChartAreas.Add(new ChartArea());
                    Charts[row, column].Dock = DockStyle.Fill;
                    table.Controls.Add(Charts[row, column], column, row);
                }

            
            form = new Form()
            {
                Text = name,
                Size = new Size(800, 600),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                WindowState = FormWindowState.Maximized
            };


            table.Dock = DockStyle.Fill;
            table.Size = form.ClientSize;
            form.Controls.Add(table);


            bool algorithmExited = false;

            task.UpdateCharts += (series,index)=>
            {
              //  try                {
                    form.BeginInvoke(new Action<List<Series>, int>(UpdateCharts), series, index);
               // }                catch { }
            };

            task.Exit += () => { algorithmExited = true;  try { form.BeginInvoke(new Action(form.Close)); } catch { } };

            form.FormClosing += (s, a) =>
              {
                  task.RequestExit();
                  a.Cancel = !algorithmExited;
              };

            new Action(task.Run).BeginInvoke(null, null);
            Application.Run(form);
        }
    }
}
