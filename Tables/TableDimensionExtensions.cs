using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public static class TableDimensionExtensions
    {
        public class BinSearchReply
        {
            public bool ExactMatch;
            public int Index;
        }



        public static BinSearchReply BinSearch<T>(this TableDimension<T> dim, T element)
            where T : IComparable
        {
            if (dim.Values.Count == 0) return new BinSearchReply { ExactMatch = false, Index = 0 };
            var array = dim.Values;
            var left = 0;
            var right = array.Count;
            while (left < right)
            {
                var middle = (right + left) / 2;
                if (element.CompareTo(array[middle]) <= 0)
                    right = middle;
                else left = middle + 1;
            }
            var result = new BinSearchReply();
            result.ExactMatch = array.Count>right && array[right].CompareTo(element) == 0;
            result.Index = right;
            return result;
        }

    }
}
