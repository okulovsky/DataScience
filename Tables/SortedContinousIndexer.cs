using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public class SortedContinousIndexer<T> : IDimenstionIndexer<T, T>
       where T : IComparable
    {
        TableDimension<T> dim;
        Func<T, T> getNext;
        public SortedContinousIndexer(TableDimension<T> dim, Func<T, T> getNext)
        {
            this.dim = dim;
            this.getNext = getNext;
        }

        public T GetValue(T value)
        {
            return value;
        }

        public T Touch(T value)
        {
            if (dim.Contains(value)) return value;

            if (dim.Values.Count == 0)
            {
                dim.Add(value);
                return value;
            }

            if (value.CompareTo(dim.Values[0]) < 0)
            {
                int position = 0;
                while (value.CompareTo(dim.Values[position]) < 0)
                {
                    dim.Insert(value, position);
                    position++;
                    value = getNext(value);
                }
                return value;
            }

            if (value.CompareTo(dim.Values[dim.Values.Count - 1]) > 0)
            {
                var v = dim.Values[dim.Values.Count - 1];
                while (v.CompareTo(value) < 0)
                {
                    v = getNext(v);
                    dim.Add(v);
                }
                return value;
            }

            throw new ArgumentException($"{value} is not greater that last and smaller that 0-th element, yet it is not found");
        }
    }

}
