using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    [TestFixture]
    class SortedContinousIndexer_Should
    {
        [Test]
        public void NotModifyIfValueExists()
        {
            var values = new int[] { 1, 2, 3 };
            var dim = new TableDimension<int>(values);
            new SortedContinousIndexer<int>(dim, z => z + 1).Touch(2);
            CollectionAssert.AreEqual(values, dim.Values);
        }

        [Test]
        public void AddToLeft()
        {
            var values = new int[] { 1, 2, 3 };
            var dim = new TableDimension<int>(values);
            new SortedContinousIndexer<int>(dim, z => z + 1).Touch(-1);
            CollectionAssert.AreEqual(new[] { -1, 0, 1, 2, 3 }, dim.Values);
        }

        [Test]
        public void AddToRight()
        {
            var values = new int[] { 1, 2, 3 };
            var dim = new TableDimension<int>(values);
            new SortedContinousIndexer<int>(dim, z => z + 1).Touch(5);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5}, dim.Values);
        }


        [Test]
        public void NotAddToMiddle()
        {
            var values = new int[] { 1,3 };
            var dim = new TableDimension<int>(values);
            Assert.Throws(
                typeof(ArgumentException), 
                ()=>new SortedContinousIndexer<int>(dim, z => z + 2).Touch(2));
        }
    }
}
