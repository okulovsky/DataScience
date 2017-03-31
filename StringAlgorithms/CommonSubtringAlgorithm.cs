using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{
    public class CommonSubtringAlgorithm : IStringComparator
    {
        public IEnumerable<Tuple<int,int,int>> MakeMatches<T>(List<T> first, List<T> second, Func<T, T, bool> comparer)
        {
            var prev = new int[second.Count + 1];
            var next = new int[second.Count + 1];



            for (int i = 0; i < first.Count; i++)
            {
                next[0] = 0;
                for (int j = 0; j < second.Count; j++)
                {
                    var eq = comparer(first[i], second[j]);
                    next[j + 1] = eq ? (prev[j]+1): 0;
                    if (eq && (j == second.Count - 1 || i == first.Count - 1))
                        yield return Tuple.Create(i, j, next[j + 1]);
                    if (!eq && prev[j] > 0)
                        yield return Tuple.Create(i - 1, j - 1, prev[j]);
                }
                var t = prev;
                prev = next;
                next = t;
            }
        }

        public IEnumerable<Tuple<int, int>> Perform<T>(List<T> first, List<T> second, Func<T, T, bool> comparer)
        {
            if (first.Count == 0 || second.Count == 0) yield break;

            var matches = MakeMatches(first, second, comparer).ToList();
            if (matches.Count == 0) yield break;
            var longest = matches.ArgMax(z => z.Item3);
            for (int i = -longest.Item3 + 1; i <=0; i++)
                yield return Tuple.Create(longest.Item1 + i, longest.Item2 + i);

        }
    }
}
