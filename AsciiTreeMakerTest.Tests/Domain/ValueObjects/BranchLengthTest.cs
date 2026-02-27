using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMakerTest.Tests.Domain.ValueObjects
{
    [TestClass]
    public class BranchLengthTest
    {
        [TestMethod]
        public void 枝の長さ_Equals()
        {
            var x = new BranchLength(1);
            var y = new BranchLength(1);
            Assert.AreEqual(true, x.Equals(y));
        }

        [TestMethod]
        public void 枝の長さ_同値()
        {
            var x = new BranchLength(2);
            var y = new BranchLength(2);
            Assert.AreEqual(true, x == y);
        }

        [TestMethod]
        public void 枝の長さ_下限値()
        {
            var b = new BranchLength(0);
            Assert.AreEqual(0, b.Value);
        }

        [TestMethod]
        public void 枝の長さ_上限値()
        {
            var b = new BranchLength(5);
            Assert.AreEqual(5, b.Value);
        }

        [TestMethod] 
        public void 枝の長さ_下限未満は例外()
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new BranchLength(-1));
            StringAssert.Contains(
                ex.Message,
                "枝の長さは0から5の間の整数でなければなりません");
        }

        [TestMethod]
        public void 枝の長さ_上限超過は例外()
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new BranchLength(6));
        }
    }
}
