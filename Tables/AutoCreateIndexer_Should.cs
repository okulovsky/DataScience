using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    [TestFixture]
    class AutoCreateIndexer_Should
    {
        [Test]
        public void AddNonExisted()
        {
            var dim = new TableDimension<int>(1, 2);
            Assert.AreEqual(3, new AutoCreateIndexer<int>(dim).Touch(3));
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, dim.Values);
        }

        [Test]
        public void DontAddExisted()
        {
            var dim = new TableDimension<int>(1, 2);
            Assert.AreEqual(new AutoCreateIndexer<int>(dim).Touch(2), 2);
            CollectionAssert.AreEqual(new[] { 1, 2 }, dim.Values);

        }
    }

}
