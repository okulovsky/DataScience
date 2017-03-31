using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public class SortedAutoCreateIndexer<T> : IDimenstionIndexer<T, T>
        where T : IComparable
    {
        TableDimension<T> dim;
        public SortedAutoCreateIndexer(TableDimension<T> dim)
        {
            this.dim = dim;
        }

        public T Touch(T value)
        {
            if (value.Equals(1) && dim.Count() == 1)
                Console.WriteLine();
            var index = dim.BinSearch(value);
            if (index.ExactMatch) return value;
            dim.Insert(value, index.Index);
            return value;
        }
    }
}
