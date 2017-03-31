using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public class IntIndexer<T> : IDimenstionIndexer<T, int>
    {
        TableDimension<T> dim;

        public IntIndexer(TableDimension<T> dim)
        {
            this.dim = dim;
        }


        public T Touch(int value)
        {
            if (value < 0 || value >= dim.Values.Count)
                throw new ArgumentException($"Position {value} is not found in {dim.Name}");
            return dim.GetValue(value);
        }
    }
}
