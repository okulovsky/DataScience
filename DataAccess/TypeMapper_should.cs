using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    [TestFixture]
    class TypeMapper_should
    {
        class A
        {
            public int Q;
            public string W { get; set; }
        }


        [Test]
        public void Work()
        {
            var mapper = new TypeMapper<A>(z => z.Q, z => z.W);
            var a = mapper.Produce(new object[] { 12, "12" });
            Assert.AreEqual(12, a.Q);
            Assert.AreEqual("12", a.W);
        }
    }
}
