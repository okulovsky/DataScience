using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    [TestFixture]
    public class NormalizerExtensionTest
    {
        [Test]
        public static void NormalizationTest()
        {
            var c = 10;
            var source = Enumerable.Range(0, c).Select(z => new
            {
                A = 2 * (double)z + 100,
                B = (double)z,
                C = (double)z,
                D = z.ToString()
            }).ToList();
            var result = source.Normalize((z, norm) => new
            {
                A = norm.Normalize(x => x.A),
                B = norm.Normalize(x => x.A),
                C = z.C,
                D = z.D
            }).ToList();
            var expected = Enumerable.Range(0, c).Select(z => (double)z /(c-1)).ToList();
            CollectionAssert.AreEqual(expected, result.Select(z => z.A));
            CollectionAssert.AreEqual(expected, result.Select(z => z.B));
            CollectionAssert.AreEqual(source.Select(z => z.C).ToList(), result.Select(z => z.C).ToList());
            CollectionAssert.AreEqual(source.Select(z => z.D).ToList(), result.Select(z => z.D).ToList());


        }
    }


    public static class NormalizeExtension
    {
        public static IEnumerable<TResult> Normalize<TSource,TResult>(this IEnumerable<TSource> _en, Func<TSource,Normalizer<TSource>,TResult> selector)
        {
            var en = _en.ToList();
            var norm = new Normalizer<TSource>();
            foreach (var e in en)
            {
                norm.currentObject = e;
                selector(e, norm);
            }
            norm.firstPass = false;
            foreach (var e in en)
            {
                norm.currentObject = e;
                yield return selector(e, norm);
            }
        }
    }

    public class Normalizer<T>
    {
        Dictionary<string, Func<T, double>> selectors = new Dictionary<string, Func<T, double>>();
        Dictionary<string, double> mins = new Dictionary<string, double>();
        Dictionary<string, double> maxes = new Dictionary<string, double>();

        public bool firstPass = true;
        public T currentObject;

        public double Normalize(Expression<Func<T,double>> norm)
        {
            var key = norm.ToString();
            if (!selectors.ContainsKey(key)) selectors[key] = norm.Compile();
            if (firstPass)
            {
                var value = selectors[key](currentObject);
                if (!mins.ContainsKey(key)) mins[key] = value;
                else mins[key] = Math.Min(mins[key], value);
                if (!maxes.ContainsKey(key)) maxes[key] = value;
                else maxes[key] = Math.Max(maxes[key], value);
                return 0;
            }
            else
            {
                var value = selectors[key](currentObject);
                return (value - mins[key]) / (maxes[key] - mins[key]);
            }
        }
    }
}
