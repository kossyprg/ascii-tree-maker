using AsciiTreeMaker.Domain.Exceptions;
using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMakerTest.Tests.Domain.ValueObjects
{
    [TestClass]
    public class NodeNameTest
    {
        [TestMethod]
        public void ノード名_Equals()
        {
            var x = new NodeName("root");
            var y = new NodeName("root");
            Assert.AreEqual(true, x.Equals(y));
        }

        [TestMethod]
        public void ノード名_同値()
        {
            var x = new NodeName("root");
            var y = new NodeName("root");
            Assert.AreEqual(true, x == y);
        }

        [TestMethod]
        public void ノード名_空欄あるいは空白文字は例外()
        {
            var ex = Assert.ThrowsException<NodeNameIsEmptyException>(
                () => new NodeName("    "));
            StringAssert.Contains(
                ex.Message,
                "ノード名を入力してください。");
        }

        [TestMethod]
        public void ノード名_文字数超過は例外()
        {
            var ex = Assert.ThrowsException<NodeNameTooLongException>(
                () => new NodeName("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901"));
            StringAssert.Contains(
                ex.Message,
                "ノード名は100文字以内で入力してください。");
        }
    }
}
