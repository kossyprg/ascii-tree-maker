using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Domain.Repositories
{
    /// <summary>
    /// 木構造データのインタフェース
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// ノードのもつ値
        /// </summary>
        NodeName Name { get; }

        /// <summary>
        /// 子ノード
        /// </summary>
        IReadOnlyList<ITreeNode> Children { get; }
    }
}
