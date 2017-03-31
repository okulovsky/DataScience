using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public class DimensionIndexerFactory<T>
    {
        public readonly TableDimension<T> dim;
        public DimensionIndexerFactory(TableDimension<T> dim)
        {
            this.dim = dim;
        }

       
    }

    public static class DimentionIndexerExtensions
    {
        public static OnlyExistedIndexer<T> OnlyExisted<T>(this DimensionIndexerFactory<T> factory)
        {
            return new OnlyExistedIndexer<T>(factory.dim);
        }

        public static IntIndexer<T> Int<T>(this DimensionIndexerFactory<T> factory)
        {
            return new IntIndexer<T>(factory.dim);
        }

        public static AutoCreateIndexer<T> AutoCreate<T>(this DimensionIndexerFactory<T> factory)
        {
            return new AutoCreateIndexer<T>(factory.dim);
        }

        public static SortedAutoCreateIndexer<T> SortedAutoCreate<T>(this DimensionIndexerFactory<T> factory)
            where T : IComparable
        {
            return new SortedAutoCreateIndexer<T>(factory.dim);
        }

        public static SortedContinousIndexer<T> SortedContinous<T>(this DimensionIndexerFactory<T> factory, Func<T,T> getNext)
            where T : IComparable
        {
            return new SortedContinousIndexer<T>(factory.dim, getNext);
        }
    }

}
