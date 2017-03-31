using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{
    [TestFixture]
    class CommonSubstringAlgorithmTests
    {
        [TestCase("abc", "xbcy", "bc")]
        [TestCase("aaaabc", "xbcy", "bc")]
        [TestCase("aaaabcty", "xbcy", "bc")]
        public void TestCSA(string first, string second, string common)
        {
            var alg = new CommonSubtringAlgorithm();
            var e = alg.Perform(first.ToList(), second.ToList(), (q, w) => q == w).ToList();
            var f1 = e.Select(z => first[z.Item1].ToString()).Aggregate((a, b) => a + b);
            var f2 = e.Select(z => second[z.Item2].ToString()).Aggregate((a, b) => a + b);
            Assert.AreEqual(f1, f2);
            Assert.AreEqual(f1, common);
        }
    }
}
