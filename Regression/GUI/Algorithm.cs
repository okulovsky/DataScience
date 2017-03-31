using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataScience
{
    public abstract class AbstractAlgorithm 
    {
        public event Action<double> RegisterError;
        protected void OnRegisterError(double value)
        {
            if (RegisterError != null)
                RegisterError(value);
        }

        public event Action<List<Series>,int> UpdateCharts;
        protected void OnUpdateCharts(List<Series> series, int chartIndex=0)
        {
            if (UpdateCharts != null)
                UpdateCharts(series,chartIndex);
        }

        public bool ExitRequested { get; protected set; }

        public void RequestExit()
        {
            ExitRequested = true;
        }

        protected abstract void Learn();
        public void Run()
        {
            Learn();
            if (Exit != null)
                Exit();
        }
        public event Action Exit;
    }
}
