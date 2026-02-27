namespace AsciiTreeMaker.Domain.ValueObjects
{
    /// <summary>
    /// ノード数
    /// </summary>
    public sealed class NodeCount : ValueObject<NodeCount>
    {
        public long Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">ノード数</param>
        public NodeCount(long value)
        {
            if (value < 0)
            {
                string message = "ノード数は非負でなければなりません";
                throw new ArgumentOutOfRangeException(nameof(value), message);
            }

            Value = value;
        }

        /// <summary>
        /// 重たい処理の実行前に警告を出すノード数のしきい値
        /// </summary>
        public const long WarningThreshold = 500;

        /// <summary>
        /// 重たい処理を無効にするノード数のしきい値
        /// </summary>
        public const long DisableThreshold = 3000;

        /// <summary>
        /// 操作に関するレベル
        /// </summary>
        public enum OperationsLevel
        {
            /// <summary>
            /// 通常
            /// </summary>
            Enable,

            /// <summary>
            /// 可能だが処理が重くなる可能性があるため警告する
            /// </summary>
            Warning,

            /// <summary>
            /// 処理が重たいため無効にする
            /// </summary>
            Disable
        }

        /// <summary>
        /// 操作に関するレベルを取得する
        /// </summary>
        /// <returns>操作に関するレベル</returns>
        public OperationsLevel GetOperationsLevel()
        {
            if (Value >= DisableThreshold)
            {
                return OperationsLevel.Disable;
            }

            if (Value >= WarningThreshold)
            {
                return OperationsLevel.Warning;
            }

            return OperationsLevel.Enable;
        }

        /// <summary>
        /// 個数を加算する
        /// </summary>
        /// <param name="count">加算する個数</param>
        /// <returns>加算後の個数</returns>
        public NodeCount Add(long count)
        {
            return new NodeCount(Value + count);
        }

        /// <summary>
        /// 個数を減算する
        /// </summary>
        /// <param name="count">減算する個数</param>
        /// <returns>減算後の個数</returns>
        public NodeCount Subtract(long count)
        {
            return new NodeCount(Value - count);
        }

        protected override bool EqualsCore(NodeCount other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }
    }
}
