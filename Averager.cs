using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public class Averager
    {
        Queue<double> values = new Queue<double>();
        double sum = 0;
        readonly int size;
        public Averager(int size) { this.size = size; }
        public double? Add(double value)
        {
            sum += value;
            values.Enqueue(value);
            if (values.Count > size)
                return Remove();
            return null;
        }
        public double Remove()
        {
            var removed = values.Dequeue();
            sum -= removed;
            return removed;
        }

        public double Average { get { return sum / values.Count; } }
    }
}
