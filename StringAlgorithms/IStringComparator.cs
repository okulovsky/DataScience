using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{
    public interface IStringComparator
    {
        IEnumerable<Tuple<int, int>> Perform<T>(List<T> first, List<T> second, Func<T, T, bool> comparer);
    }
}
