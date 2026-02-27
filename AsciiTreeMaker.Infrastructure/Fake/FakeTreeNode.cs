using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Infrastructure.Fake
{
    /// <summary>
    /// Fake 用の最小限のノード実装
    /// </summary>
    internal sealed class FakeTreeNode : ITreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">ノードの名前</param>
        /// <param name="children">ノードが持つ子ノード群</param>
        internal FakeTreeNode(string name, params FakeTreeNode[] children)
        {
            Name = new NodeName(name);
            Children = children;
        }

        /// <summary>
        /// 子ノードの読み取り専用コレクション
        /// </summary>
        public IReadOnlyList<ITreeNode> Children { get; }

        public NodeName Name { get; }
    }
}
