﻿using DataScience;
using System;
using System.Collections.Generic;
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


    public static class IEnumerableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> en)
        {
            var set = new HashSet<T>();
            foreach (var e in en)
                set.Add(e);
            return set;
        }

        public static void ForEach<T>(this IEnumerable<T> en, Action<T> action)
        {
            foreach (var e in en)
                action(e);
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




        public class ItemWithIndex<T>
        {
            public T Item;
            public int Index;
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