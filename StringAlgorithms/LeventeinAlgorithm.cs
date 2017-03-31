using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{



    public class LeventeinAlgorithm : IStringComparator
    {
        struct C
        {
            public int result;
            public Point previous;
            public bool match;
        }




        public  IEnumerable<Tuple<int,int>> Perform<T>(List<T> first, List<T> second, Func<T, T, bool> comparer)
        {
            var matrix = new C[first.Count+1, second.Count + 1];
            for (int i = 0; i <= second.Count; i++)
                matrix[0, i] = new C { result = i, previous = new Point(-1, -1) };
            for (int i = 0; i <= first.Count; i++)
                matrix[i, 0] = new C { result = i, previous = new Point(-1, -1) };

            for (int i=1;i<=first.Count;i++)
            {
                for (int j=1;j<=second.Count;j++)
                {
                    var diff = comparer(first[i - 1], second[j - 1])?0:2;
                    var fromDia = matrix[i - 1, j - 1].result + diff;
                    var fromLeft = matrix[i, j - 1].result+1;
                    var fromTop = matrix[i - 1, j].result+1;
                    var min = Math.Min(Math.Min(fromDia, fromTop), fromLeft);
                    matrix[i, j].result = min;

                    if (min == fromDia)
                    {
                        matrix[i, j].previous = new Point(i - 1, j - 1);
                        matrix[i, j].match = diff == 0;
                    }
                    else if (min == fromTop)
                        matrix[i, j].previous = new Point(i - 1, j);
                    else
                        matrix[i, j].previous = new Point(i, j - 1);
                }
            }

            if (false)
            for (int i = 0; i <= first.Count; i++)
            {
                for (int j = 0; j <= second.Count; j++)
                    Console.Write(matrix[i, j].result+ (matrix[i,j].match?"!":" ")+" ");

                Console.WriteLine();
            }
            var result = new List<Tuple<int, int>>();
            var p = new Point(first.Count, second.Count);
            while (p.X > 0 && p.Y > 0)
            {
                if (matrix[p.X, p.Y].match)
                    result.Add(Tuple.Create(p.X-1, p.Y-1));
                p = matrix[p.X, p.Y].previous;
            }
            result.Reverse();
            return result;
        }
    }
}
