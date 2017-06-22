using DataScience;
using DataScience;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    [Serializable]
    public class MeanAndSTD
    {
        public double Mean;
        public double Std;
    }

    public class ItemWithIndex<T>
    {
        public T Item;
        public int Index;
    }


    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> AggregateAdd<T>(this IEnumerable<T> data, Func<T,T,T> aggregator)
        {
            var accumulator = default(T);
            var firstTime = true;
            foreach(var e in data)
            {
                if (firstTime)
                {
                    accumulator = e;
                    firstTime = false;
                }
                else
                {
                    accumulator = aggregator(accumulator, e);
                }
                yield return accumulator;
            }
        }


        public static string Join(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings);
        }

        public static Pair<T> AsPair<T>(this IEnumerable<T> data)
        {
            var list = data.ToList();
            if (list.Count != 2)
                throw new ArgumentException();
            return new Pair<T>(list[0], list[1]);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> en, params T[] data)
        {
            return data.Concat(en);
        }


        public static IEnumerable<T> WithCounts<T>(this IEnumerable<T> en, int st=100)
        {
            int cnt = 0;
            foreach (var e in en)
            {
                yield return e;
                if (cnt % st == 0)
                    Console.Write(cnt + "               \r");
                cnt++;
            }
            Console.WriteLine();
        }

        public static IEnumerable<T> WithReporting<T>(this IEnumerable<T> en, Func<T,string> reportMaker, int st=1)
        {
            int cnt = 0;
            foreach (var e in en)
            {
                yield return e;
                if (cnt % st == 0)
                    Console.WriteLine(reportMaker(e));
                cnt++;
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> en, Action<T> action)
        {
            foreach (var e in en)
            {
                action(e);
                yield return e;
            }
        }


        public static string ToConsole(this object obj)
        {
            string result = null;
            if (obj == null)
                result = "NULL";
            else if (obj.GetType().IsPrintable())
                result = obj.ToString();
            else if (obj is IEnumerable)
            {
                result = "";
                foreach (object c in obj as IEnumerable)
                    result += c.ToString() + "\n";
            }
            if (result==null)
                throw new Exception("Cannot output to console. Use Print instead");
            Console.WriteLine(result);
            return result;
        }

        public static void ToFile<T>(this IEnumerable<T> en, string filename)
        {
            using (var file = new StreamWriter(filename,false, Encoding.UTF8))
                foreach (var e in en)
                    file.WriteLine(e.ToString());
        }

        public class AddSelectResult<T1,T2>
        {
            public readonly T1 Original;
            public readonly T2 Addition;
            public AddSelectResult(T1 original, T2 addition)
            {
                Original = original;
                Addition = addition;
            }
        }

        public static IEnumerable<AddSelectResult<T1,T2>> AddSelect<T1,T2>(this IEnumerable<T1> en, Func<T1,T2> selector)
        {
            foreach (var e in en)
                yield return new AddSelectResult<T1, T2>(e, selector(e));
        }

        public static IEnumerable<T1> WhereAdded<T1,T2>(this IEnumerable<AddSelectResult<T1,T2>> en, Func<T2,bool> filter)
        {
            foreach (var e in en)
                if (filter(e.Addition))
                    yield return e.Original;
        }


        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> en)
        {
            var set = new HashSet<T>();
            foreach (var e in en)
                set.Add(e);
            return set;
        }


        public class CountByResult<TKey>
        {
            public TKey Key;
            public int Count;
            public override string ToString()
            {
                return Key.ToString() + ": " + Count;
            }
        }

        public static IQueryable<CountByResult<TKey>> CountBy<TData, TKey>(this IQueryable<TData> data, Expression<Func<TData, TKey>> key)
        {
            return data.GroupBy(key).Select(z => new CountByResult<TKey> { Key = z.Key, Count = z.Count() }).OrderByDescending(z => z.Count);
        }

        public static IEnumerable<CountByResult<TKey>> CountBy<TData, TKey>(this IEnumerable<TData> data, Func<TData, TKey> key)
        {
            return data.GroupBy(key).Select(z => new CountByResult<TKey> { Key = z.Key, Count = z.Count() }).OrderByDescending(z => z.Count);
        }

        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key)
            where TValue : new()
        {
            if (!d.ContainsKey(key)) d[key] = new TValue();
            return d[key];
        }

        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, Func<TValue> createValue)
        {
            if (!d.ContainsKey(key)) d[key] = createValue();
            return d[key];
        }

        public static void SafeReplace<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, Func<TValue, TValue> replacer)
            where TValue : new()
        {
            if (!d.ContainsKey(key)) d[key] = new TValue();
            d[key] = replacer(d[key]);
        }







        public static IEnumerable<ItemWithIndex<T>> WithIndices<T>(this IEnumerable<T> en)
        {
            int index = 0;
            foreach (var e in en)
                yield return new ItemWithIndex<T> { Item = e, Index = index++ };
        }



        public static IEnumerable<int> Which<T>(this IEnumerable<T> data, Func<T, bool> predicate)
        {
            int count = 0;
            foreach (var e in data)
            {
                if (predicate(e)) yield return count;
                count++;
            }
        }

        [Obsolete]
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> data)
        {
            var previous = default(T);
            var firstTime = true;
            foreach (var e in data)
            {
                if (!firstTime)
                    yield return Tuple.Create(previous, e);
                previous = e;
                firstTime = false;
            }
        }

        [Obsolete]
        public static int IndexOfMax<T>(this IEnumerable<T> data, Func<T, double> selector)
        {
            int result = 0;
            double value = double.NegativeInfinity;
            int counter = 0;
            foreach (var e in data)
            {
                var newValue = selector(e);
                if (newValue > value)
                {
                    value = newValue;
                    result = counter;
                }
                counter++;
            }
            return result;
        }


        public static Histogram<double, object> ToHistogram(this IEnumerable<double> data, double precision)
        {
            var header = new object();
            var hist = new Histogram<double, object>(z => z * precision);
            foreach (var e in data)
                hist.AddValue(header, (int)(e / precision), 1);
            return hist;
        }

        public static TData ArgMaxOrDefault<TData, TCompare>(this IEnumerable<TData> data, Func<TData, TCompare> selector)
            where TCompare : IComparable
        {
            return data.ArgMax(selector, true);
        }

        public static TData ArgMax<TData,TCompare>(this IEnumerable<TData> data, Func<TData, TCompare> selector, bool OrDefault=false)
            where TCompare : IComparable
        {
            TData result = default(TData);
            bool firstTime = true;
            TCompare current = default(TCompare);

            foreach (var e in data)
            {
                if (firstTime)
                {
                    current = selector(e);
                    result = e;
                    firstTime = false;
                    continue;
                }

                var newValue = selector(e);
                if (newValue.CompareTo(current)>0)
                {
                    current = newValue;
                    result = e;
                }
            }

            if (firstTime && !OrDefault)
                    throw new ArgumentException("Sequence contains no elements");

            return result;
        }

        public static MeanAndSTD MeanAndStd(this IEnumerable<double> _en)
        {
            var en = _en.ToList();
            var result = new MeanAndSTD();
            if (en.Count <2 )
            {
                result.Mean = double.NaN;
                result.Std = double.NaN;
                return result;
            }
            result.Mean = en.Average();
            var num = 0.0;
            foreach (var e in en)
                num += Math.Pow(e - result.Mean, 2);
            result.Std = Math.Sqrt(num / (en.Count - 1));
            return result;
        }
        public static IEnumerable<TData> Distinct<TData,TField>(this IEnumerable<TData> data, Func<TData,TField> value)
        {
            var set = new HashSet<TField>();
            foreach(var e in data)
            {
                var v = value(e);
                if (set.Contains(v))
                    continue;
                set.Add(v);
                yield return e;
            }
        }

        
        public static T MaxOrDefault<T>(this IEnumerable<T> en)
            where T : IComparable
        {
            var firstTime = true;
            var max = default(T);
            foreach (var e in en)
            {
                if (firstTime)
                {
                    max = e;
                    firstTime = false;
                }
                else
                {
                    if (e.CompareTo(max) > 0) max = e;
                }
            }
            return max;
        }
    }
}
