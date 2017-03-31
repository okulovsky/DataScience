using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{
    public class Joiner
    {
        IStringComparator comparator;

        public Joiner(IStringComparator comparator)
        {
            this.comparator = comparator;
        }

        public class JoinResult<T1, T2>
        {
            public T1 Item1;
            public T2 Item2;
            public double Likeness;
        }

        public IEnumerable<JoinResult<T1,T2>> Join<T1,T2>(IEnumerable<T1> _e1, IEnumerable<T2> _e2, Func<T1,string> selector1, Func<T2,string> selector2)
        {
            var e1 = _e1.ToList();
            var e2 = _e2.ToList();
            var s1 = e1.Select(z=>selector1(z).ToList()).ToList();
            var s2 = e2.Select(z=>selector2(z).ToList()).ToList();
            var matrix = new double[s1.Count, s2.Count];
            for (int x = 0; x < s1.Count; x++)
                for (int y = 0; y < s2.Count; y++)
                {
                    double matchCount = comparator.Perform(s1[x], s2[y], (a, b) => a == b).Count();
                    matrix[x, y] = 2 * matchCount / (s1[x].Count + s2[y].Count);
                }
            var pairs = Pairwise.Greedy(matrix).ToList();
            foreach (var e in pairs)
                yield return new JoinResult<T1, T2> { Item1 = e1[e.Item1], Item2 = e2[e.Item2], Likeness = matrix[e.Item1, e.Item2] };
        }

        public IEnumerable<JoinResult<T1, T2>> LeftJoin<T1, T2>(IEnumerable<T1> _e1, IEnumerable<T2> _e2, Func<T1, string> selector1, Func<T2, string> selector2, double minimalMatch)
        {
            var j = Join(_e1, _e2, selector1, selector2);
            var set = _e1.ToHashSet();
            foreach (var e in j)
            {
                if (e.Likeness >= minimalMatch)
                {
                    yield return e;
                    set.Remove(e.Item1);
                }
            }
            foreach (var e in set)
            {
                yield return new JoinResult<T1, T2> { Item1 = e, Item2 = default(T2), Likeness = 0 };
            }
        }
    }
}
