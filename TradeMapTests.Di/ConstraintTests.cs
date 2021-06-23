using Xunit;
using TradeMap.Di.Constraints;
using System.Collections.Generic;

namespace TradeMapTests.Di
{
    public class ConstraintTests
    {
        [Fact()]
        public void ConstraintAllTest()
        {
            var cons = new ConstraintAll();
            Assert.True(cons.Check("asdfg"));
            Assert.True(cons.Check("1"));
            Assert.True(cons.Check("1.5"));
        }

        [Fact()]
        public void ConstraintInt()
        {
            var cons = new ConstraintInt();
            Assert.False(cons.Check("asdfg"));
            Assert.True(cons.Check("1"));
            Assert.False(cons.Check("1.5"));
        }

        [Fact()]
        public void ConstraintDouble()
        {
            var cons = new ConstraintDouble();
            Assert.False(cons.Check("asdfg"));
            Assert.True(cons.Check("1"));
            Assert.True(cons.Check("1.5"));
        }

        [Fact()]
        public void ConstraintMinMax()
        {
            var cons = new ConstraintMinMax("0 2");
            Assert.False(cons.Check("asdfg"));
            Assert.True(cons.Check("1"));
            Assert.True(cons.Check("1.5"));
            Assert.True(cons.Check("0"));
            Assert.True(cons.Check("2"));
            Assert.False(cons.Check("-1"));
            Assert.False(cons.Check("3"));
        }
    }
}