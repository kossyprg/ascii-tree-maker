namespace AsciiTreeMaker.Domain.ValueObjects
{
    /// <summary>
    /// ノードの階層
    /// </summary>
    public sealed class NodeDepth : ValueObject<NodeDepth>
    {
        public int Value { get; }

        /// <summary>
        /// 最大階層
        /// </summary>
        public const int MAX = 10;

        protected override bool EqualsCore(NodeDepth other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }
    }
}
