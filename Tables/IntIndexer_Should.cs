using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    [TestFixture]
    class IntIndexer_Should
    { 

        [Test]
        public void DontTouchNonExisted()
        {
            var dim = new TableDimension<int>(0, 1, 2, 3);
            Assert.Throws(typeof(ArgumentException), () => new IntIndexer<int>(dim).Touch(5));
        }

        [Test]
        public void DontTouchNegative()
        {
            var dim = new TableDimension<int>(0, 1, 2, 3);
            Assert.Throws(typeof(ArgumentException), () => new IntIndexer<int>(dim).Touch(-5));
        }


        [Test]
        public void TouchExisted()
        {
            var dim = new TableDimension<string>("A", "B", "C");
            Assert.AreEqual("B", new IntIndexer<string>(dim).Touch(1));
        }
    }
}
