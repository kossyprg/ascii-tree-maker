using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMakerTest.Tests.Domain.ValueObjects
{
    [TestClass]
    public class NodeCountTest
    {
        [TestMethod]
        public void ノード数_Equals()
        {
            var x = new NodeCount(1);
            var y = new NodeCount(1);
            Assert.AreEqual(true, x.Equals(y));
        }

        [TestMethod]
        public void ノード数_同値()
        {
            var x = new NodeCount(2);
            var y = new NodeCount(2);
            Assert.AreEqual(true, x == y);
        }

        [TestMethod]
        public void ノード数_加算()
        {
            var x = new NodeCount(3);
            var y = x.Add(7);
            Assert.AreEqual(10, y.Value);
        }

        [TestMethod]
        public void ノード数_減算()
        {
            var x = new NodeCount(13);
            var y = x.Subtract(4);
            Assert.AreEqual(9, y.Value);
        }

        [TestMethod] 
        public void ノード数_操作に関するレベル()
        {
            var n1 = new NodeCount(499);
            Assert.AreEqual(NodeCount.OperationsLevel.Enable, n1.GetOperationsLevel());

            var n2 = new NodeCount(500);
            Assert.AreEqual(NodeCount.OperationsLevel.Warning, n2.GetOperationsLevel());

            var n3 = new NodeCount(2999);
            Assert.AreEqual(NodeCount.OperationsLevel.Warning, n3.GetOperationsLevel());

            var n4 = new NodeCount(3000);
            Assert.AreEqual(NodeCount.OperationsLevel.Disable, n4.GetOperationsLevel());
        }

        [TestMethod]
        public void ノード数_負の値は例外()
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new NodeCount(-1));
            StringAssert.Contains(
                ex.Message,
                "ノード数は非負でなければなりません");
        }
    }
}
