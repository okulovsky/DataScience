using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class Indexer : IEnumerable<double>
    {
        List<int> indices;
        List<double> array;
        public Indexer(List<int> indices, List<double>array)
        {
            this.indices = indices;
            this.array = array;
        }
        public double this[int index]
        {
            get { return array[indices[index]]; }
            set { array[indices[index]] = value; }
        }

        public IEnumerator<double> GetEnumerator()
        {
            return indices.Select(z => array[z]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count {  get { return indices.Count; } }
    }
}
