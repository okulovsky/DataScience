using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{


    public class OnlyExistedIndexer<T> : IDimenstionIndexer<T, T>
    {
        TableDimension<T> dim;
        public OnlyExistedIndexer(TableDimension<T> dim)
        {
            this.dim = dim;
        }


        public T Touch(T value)
        {
            if (!dim.Contains(value))
                throw new ArgumentException($"{value} in not contained in {dim.Name}");
            return value;
        }
    }
}
