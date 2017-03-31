using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public class AutoCreateIndexer<T> : IDimenstionIndexer<T, T>
    {
        TableDimension<T> dim;
        public AutoCreateIndexer(TableDimension<T> dim)
        {
            this.dim = dim;
        }

        public T Touch(T value)
        {
            if (!dim.Contains(value)) dim.Add(value);
            return value;
        }
    }
}
