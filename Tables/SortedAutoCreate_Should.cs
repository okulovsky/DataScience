using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    [TestFixture]
    class BinSearch_Should
    {

        

        [Test]
        public void Works()
        {
            var dim = new TableDimension<int>(1, 2, 3);
            var index = dim.BinSearch(2);
            Assert.AreEqual(1, index.Index);
            Assert.AreEqual(true, index.ExactMatch);
        }

        //[Test] Этот тест не нужен, т.к. в TableDimansions все значения униклаьны
        //public void ChoosesSide()
        //{
        //    var dim = new TableDimension<int>(1, 2, 2, 2, 3);
        //    var index = dim.BinSearch(2);
        //    Assert.AreEqual(1, index.Index);
        //    Assert.AreEqual(true, index.ExactMatch);
        //}

        [Test]
        public void WorkInNonExactSituations()
        {
            var dim = new TableDimension<int>(1, 3, 5, 7);
            var index = dim.BinSearch(6);
            Assert.AreEqual(3, index.Index);
            Assert.AreEqual(false, index.ExactMatch);
            dim.Insert(6, index.Index);
            CollectionAssert.AreEqual(new[] { 1, 3, 5, 6, 7 }, dim.Values);
        }

        [Test]
        public void WorkWithOneElement1()
        {
            var dim = new TableDimension<int>(1);
            var index = dim.BinSearch(0);
            Assert.AreEqual(0, index.Index);
            Assert.False(index.ExactMatch);
        }

        [Test]
        public void WorkWithOneElement2()
        {
            var dim = new TableDimension<int>(1);
            var index = dim.BinSearch(2);
            Assert.AreEqual(1, index.Index);
            Assert.False(index.ExactMatch);
        }
    }

    [TestFixture]
    public class SortedAutoCreate_Should
    {
        [Test]
        public void AddWhenNesessary()
        {
            var dim = new TableDimension<int>(1, 3, 5, 7);
            var ind = new SortedAutoCreateIndexer<int>(dim);
            ind.Touch(2);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 5, 7 }, dim.Values);
        }

        [Test]
        public void NotAddWhenNotNesessary()
        {
            var dim = new TableDimension<int>(1, 3, 5, 7);
            var ind = new SortedAutoCreateIndexer<int>(dim);
            ind.Touch(3);
            CollectionAssert.AreEqual(new[] { 1, 3, 5, 7 }, dim.Values);

        }
    }

       
}
