using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{
    public class Pairwise
    {
        public static IEnumerable<Tuple<int, int>> Greedy(double[,] _matrix)
        {
            var matrix = (double[,])_matrix.Clone();

            while (true)
            {
                var coordinates = Enumerable
                    .Range(0, matrix.GetLength(0))
                    .SelectMany(z => Enumerable.Range(0, matrix.GetLength(1)).Select(x => new { row = z, column = x }));
                var best = coordinates.ArgMax(z => matrix[z.row, z.column]);
                if (double.IsNegativeInfinity(matrix[best.row, best.column]))
                    yield break;
                yield return Tuple.Create(best.row, best.column);
                for (int i = 0; i < matrix.GetLength(0); i++)
                    matrix[i, best.column] = double.NegativeInfinity;
                for (int i = 0; i < matrix.GetLength(1); i++)
                    matrix[best.row, i] = double.NegativeInfinity;
            }
        }
    }
}
