using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class ListDecorator : IEnumerable<double>
    {
        List<double> list;
        public ListDecorator(List<double> list)
        {
            this.list = list;
        }
        public double this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public IEnumerator<double> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count {  get { return list.Count; } }
    }
}
