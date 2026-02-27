
using AsciiTreeMaker.Domain.Exceptions;

namespace AsciiTreeMaker.Domain.ValueObjects
{
    /// <summary>
    /// ノード名
    /// </summary>
    public sealed class NodeName : ValueObject<NodeName>
    {
        public string Value { get; }

        /// <summary>
        /// ノード名の最大長
        /// </summary>
        public const int MAX_LENGTH = 100;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">ノード名</param>
        public NodeName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new NodeNameIsEmptyException();
            }

            if (value.Length > MAX_LENGTH)
            {
                throw new NodeNameTooLongException();
            }

            Value = value;
        }

        /// <summary>
        /// 初期ノード名を取得する
        /// </summary>
        /// <returns></returns>
        public static NodeName GetInitialNodeName()
        {
            return new NodeName("テキストを入力");
        }

        protected override bool EqualsCore(NodeName other)
        {
            return Value.Equals(other.Value);
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }
    }
}
