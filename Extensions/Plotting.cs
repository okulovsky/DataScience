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

        public static Series MakeHistogram<T>(this IEnumerable<T> data, Func<T, double> selector, double min, double max, int count=-1)
        {
            if (count == -1) count = (int)Math.Round(max - min);
            var values = new int[count];
            var sum = 0;
            foreach (var e in data)
            {
                var value = selector(e);
                int index = (int)(count*(value - min) / (max - min));
                if (index < 0) index = 0;
                if (index >= count) index = count - 1;
                values[index]++;
                sum++;
            }
            var serie = new Series();
            for (int i = 0; i < values.Length; i++)
                serie.Points.Add(new DataPoint(min + i * (max - min) / count, (double)values[i] / count));
            serie.WithType(SeriesChartType.Column);
            return serie;
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
