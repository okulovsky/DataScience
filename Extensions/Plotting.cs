using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataScience
{
    public static class Plotting
    {
        public static Series ToSeries<T>(this IEnumerable<T> en, Func<T, double> xSelector, Func<T, double> ySelector)
        {
            var s = new Series();
            s.Fill(en, xSelector, ySelector);
            return s;
        }

        

        public static Series ToSeries<T>(this IEnumerable<T> en, Func<T, double> ySelector)
        {
            var s = new Series() { ChartType = SeriesChartType.FastLine };

            s.Fill(en, ySelector);
            return s;
        }

        

        public static IEnumerable<KeyValuePair<double,double>> MakeHistogram<T>(this IEnumerable<T> _data, Func<T,double> selector, int bins=50)
        {
            var data = _data.Select(selector).ToList();
            var min = data.Min();
            var max = data.Max();
            var bin = (max - min) / bins;
            var counts = new int[bins];
            foreach (var e in data)
            {
                var index = (int)(bins * (e - min) / (max - min));
                if (index < 0) index = 0;
                if (index >= bins) index = bins-1;
                counts[index]++;
            }



            var result = new Series();
            for (int i = 0; i < bins; i++)
            {
                yield return new KeyValuePair<double, double>(
                    min + i * (max - min) / bins, 
                    (double)counts[i] / data.Count);
            }
        }

     
        public static Series Fill<T>(this Series s, IEnumerable<T> en, Func<T, double> ySelector)
        {
            int x = 0;
            foreach (var e in en)
            {
                s.Points.Add(new DataPoint(x, ySelector(e)));
                x++;
            }
            return s;
        }

        public static Series Fill<T>(this Series s, IEnumerable<T> en, Func<T,double> xSelector, Func<T,double> ySelector)
        {
            foreach (var e in en)
            {
                s.Points.Add(new DataPoint(xSelector(e), ySelector(e)));
            }
            return s;
        }

        public static Series WithColor(this Series s, Color color)
        {
            s.Color = color;
            return s;
        }

        public static Series WithThinkness(this Series s, int th)
        {
            s.LabelBorderWidth = th;
            s.BorderWidth = th;
            return s;
        }

        public static Series WithType(this Series s, SeriesChartType type)
        {
            s.ChartType = type;
            return s;
        }

        public static Series WithDash(this Series s, ChartDashStyle dash)
        {
            s.BorderDashStyle = dash;
            return s;
        }

        public static Series WithName(this Series s, string name)
        {
            s.Name = name;
            return s;
        }


        public static void Draw(this Series s, string name=null)
        {
            new[] { s }.Draw(name);
        }


        public static Chart ToChart(this Series s, string name=null)
        {
            return ToChart(new[] { s }, name);
        }

        public static Chart ToChart(this IEnumerable<Series> s, string name = null)
        {
            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());
            foreach (var e in s)
            {
                chart.Series.Add(e);
                e.IsVisibleInLegend = true;
            }
            chart.Name = name;
            chart.Legends.Add(new Legend());
            return chart;
        }

        public static void Draw(this Chart chart, string name=null)
        {
            var form = new Form();
            form.Controls.Add(chart);
            form.ClientSize = new Size(640, 480);
            chart.Dock = DockStyle.Fill;
            if (name != null)
                form.Text = name;
            Application.Run(form);
        }

        public static void Draw(this IEnumerable<Series> s, string name=null)
        {
            s.ToChart().Draw(name);
        }

    }
}
