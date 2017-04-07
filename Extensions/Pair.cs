using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Pair
    {
        public static Pair<T> Create<T>(T a, T b)
        {
            return new Pair<T>(a, b);
        }
    }

    public class Pair<T> : Tuple<T, T>, IEnumerable<T>
    {
        public Pair(T a, T b) : base(a, b)
        { }

        public T this[int index]
        {
            get
            {
                if (index == 0) return Item1;
                else if (index == 1) return Item2;
                throw new ArgumentException();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return Item1;
            yield return Item2;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
