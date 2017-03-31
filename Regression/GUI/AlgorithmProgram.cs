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
  public partial class AlgorithmProgram
    {
        AbstractAlgorithm task;

         Form form;
         Chart mainChart;
         HistoryChart history;
         List<double> historyBuffer = new List<double>();


        public double MinValue = -1;
        public double MaxValue = 1;

        public bool HistoryGraph = true;

        void UpdateCharts(List<Series> series)
        {
            mainChart.Series.Clear();
            foreach (var s in series)
                mainChart.Series.Add(s);
            
        }

         void UpdateHistory(double value)
        {
            historyBuffer.Add(value);
            if (historyBuffer.Count == 5)
            {
                history.AddRange(historyBuffer);
                historyBuffer.Clear();
            }
        }


        public  void Run(AbstractAlgorithm  _task, string name="")
        {
            task = _task;

            history = new HistoryChart
            {
                DotsCount = 200,
                Max=1,
                Lines =
                        {
                            new HistoryChartValueLine
                            {
                                DataFunction = { Color = Color.Blue }
                            }
                        },
                Dock = DockStyle.Bottom
            };

            mainChart = new Chart
            {
                ChartAreas =
                        {
                            new ChartArea
                            {
                             AxisY=
                                {
                                    Maximum=MaxValue,
                                    Minimum=MinValue
                                }   
                            }
                        },
                Dock = DockStyle.Fill
            };

            form = new Form()
            {
                Text = name,
                Size = new Size(800, 600),
                FormBorderStyle = FormBorderStyle.FixedDialog,
               
            };

            form.Controls.Add(mainChart);
            if (HistoryGraph)
                form.Controls.Add(history);

            bool algorithmExited = false;

            task.UpdateCharts += (series,index) => { try { form.BeginInvoke(new Action<List<Series>>(UpdateCharts), series); } catch { } };

            task.RegisterError += args => { try { form.BeginInvoke(new Action<double>(UpdateHistory), args); } catch { } };

            task.Exit += () => { algorithmExited = true;  try { form.BeginInvoke(new Action(form.Close)); } catch { } };

            form.FormClosing += (s, a) =>
              {
                  task.RequestExit();
                  a.Cancel = !algorithmExited;
              };

            new Action(task.Run).BeginInvoke(null, null);

            //Application.Run(form);
            form.ShowDialog();
        }
    }
}
