using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.StringAlgorithms
{
    [TestFixture]
    public class LevensteinAlgorithmTest
    {

        [TestCase("abcd", "bc", "bc")]
        [TestCase("abcd", "bxc", "bc")]
        [TestCase("abcd", "ebxcq", "bc")]
        [TestCase("abc", "bcr", "bc")]
        public void LevensteinAlgorithmTestRun(string first, string second, string sub)
        {
            Run(first, second, sub);
 //           Run(second, first, sub);
        }




            public void Run(string first, string second, string sub)
        {
            var alg = new LeventeinAlgorithm();
            var e = alg.Perform(first.ToList(), second.ToList(), (a, b) => a == b);
            var f1 = e.Select(z => first[z.Item1].ToString()).Aggregate((a, b) => a + b);
            var f2 = e.Select(z => second[z.Item2].ToString()).Aggregate((a, b) => a + b);
            Assert.AreEqual(f1, f2);
            Assert.AreEqual(f1, sub);
        }
    }
}
