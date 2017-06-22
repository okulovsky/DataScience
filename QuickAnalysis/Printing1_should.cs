using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    [TestFixture]
    class Printing1_should
    {
        void Check(object obj, params object[] expected)
        {
            var data = Printing1.Flatten(obj);
            var counter = 0;
            foreach(var e in data.SelectMany(z=>z))
            {
                Assert.AreEqual(e.Key, expected[counter]);
                Assert.AreEqual(e.Value, expected[counter + 1]);
                counter += 2;
            }
            Assert.AreEqual(expected.Length, counter);
        }
        [Test]
        public void FlattenObject()
        {
            Check(new { a = 1, b = 1.2 }, "a", 1, "b", 1.2);
        }

        [Test]
        public void FlattenIen()
        {
            Check(
                Enumerable.Range(1, 2).Select(z => new { a = z, b = 2 * z }),
                "a", 1, "b", 2, "a", 2, "b", 4);
        }

        [Test]
        public void FlattenIenIen()
        {
            Check(
                Enumerable.Range(1, 2)
                .Select(z => Enumerable.Range(1, z)),
                "0", 1, "0", 1, "1", 2);
        }

        [Test]
        public void FlattenDic()
        {
            Check(
                Enumerable
                    .Range(2, 2)
                    .Select(z => Enumerable.Range(z, 2).ToDictionary(x => "i" + x, x => x)),
                "i2", 2, "i3", 3, "i3", 3, "i4", 4);

        }
    }
}
