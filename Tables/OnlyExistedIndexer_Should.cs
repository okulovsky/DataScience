using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    [TestFixture]
    class OnlyExistedIndexer_Should
    {
        [Test]
        public void FindExisted()
        {
            var dim = new TableDimension<int>(1, 2);
            Assert.AreEqual(2, new OnlyExistedIndexer<int>(dim).Touch(2));
        }

        [Test]
        public void NotFindNonExisted()
        {
            var dim = new TableDimension<int>(1, 2);
            Assert.Throws(
                typeof(ArgumentException),
                ()=>new OnlyExistedIndexer<int>(dim).Touch(3));
        }
    }
}
