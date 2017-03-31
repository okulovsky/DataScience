using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataScience.Regression
{
    public class BasicRegressionAlgorithm : AbstractAlgorithm
    {
        public double XMin;
        public double XMax;
        public int XCount = 50;
        public Func<double, double> FunctionToRegress;
        public Network Network;
        public Teacher Teacher;
        public int IterationsLimit = int.MaxValue;

        double[] inputs;
        double[] answers;
        double[][] inputsToLearn;
        double[][] answersToLearn;

        override protected void Learn()
        {
            inputs = Enumerable
                .Range(0, XCount)
                .Select(z => (double)z / XCount)
                .Select(z => z * (XMax - XMin) + XMin)
                .ToArray();
            answers = inputs.Select(z => FunctionToRegress(z)).ToArray();
            inputsToLearn = inputs.Select(z => new[] { z }).ToArray();
            answersToLearn = answers.Select(z => new[] { z }).ToArray();

            int counter = 0;
            while (true)
            {
                var watch = new Stopwatch();
                watch.Start();
                while (watch.ElapsedMilliseconds < 200)
                {
                    LearningIteration();
                    OnRegisterError(GetError());
                    counter++;
                    if (counter > IterationsLimit) break;
                }
                watch.Stop();
                UpdateGraphs();
            }
        }

        double GetError()
        {
            double sum = 0;
            for (int i = 0; i < inputsToLearn.Length; i++)
            {
                var netResult = Network.Compute(inputsToLearn[i]);
                sum += Math.Abs(netResult[0] - answers[i]);
            }
            sum /= inputsToLearn.Length;
            return sum;
        }

        void LearningIteration()
        {
            Teacher.RunEpoch(Network, inputsToLearn, answersToLearn);
        }


        void UpdateGraphs()
        {
            var answersGraph = new Series() { ChartType = SeriesChartType.FastLine, Color = System.Drawing.Color.Red, BorderWidth = 2 };
            var outputGraph = new Series() { ChartType = SeriesChartType.FastLine, Color = System.Drawing.Color.Green, BorderWidth = 2 };

            for (int i = 0; i < XCount; i++)
            {
                answersGraph.Points.Add(new DataPoint(inputs[i], answers[i]));
                outputGraph.Points.Add(new DataPoint(inputs[i], Network.Compute(new[] { inputs[i] })[0]));
            }

            OnUpdateCharts(new List<Series> { answersGraph, outputGraph });
        }
    }
}
