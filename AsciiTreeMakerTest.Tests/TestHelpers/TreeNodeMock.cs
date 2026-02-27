using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMakerTest.Tests.TestHelpers
{
    /// <summary>
    /// [テスト用] 木構造モック
    /// </summary>
    public class TreeNodeMock : ITreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">ノードの名前</param>
        /// <param name="children">子ノード(可変長引数)</param>
        public TreeNodeMock(string name, params TreeNodeMock[] children)
        {
            Name = new NodeName(name);
            Children = children;
        }

        //public string Value { get; }
        public IReadOnlyList<ITreeNode> Children { get; }

        public NodeName Name { get; }
    }
}
