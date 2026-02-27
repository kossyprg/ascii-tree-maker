namespace AsciiTreeMaker.Domain.ValueObjects
{
    /// <summary>
    /// 枝の長さ
    /// </summary>
    public sealed class BranchLength : ValueObject<BranchLength>
    {
        public int Value { get; }

        /// <summary>
        /// 枝の長さの最大値
        /// </summary>
        public const int MAX = 5;

        /// <summary>
        /// 枝の長さの最小値
        /// </summary>
        public const int MIN = 0;

        /// <summary>
        /// 既定の設定値
        /// Linux の tree コマンドに合わせる
        /// </summary>
        public static readonly BranchLength Default = new(2);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">枝の長さ</param>
        public BranchLength(int value)
        {
            if (value < MIN || value > MAX)
            {
                string message = $"枝の長さは{MIN}から{MAX}の間の整数でなければなりません";
                throw new ArgumentOutOfRangeException(nameof(value), message);
            }

            Value = value;
        }

        /// <summary>
        /// 枝の長さの取りうる候補
        /// </summary>
        public static IReadOnlyList<int> Candidates { get; } = Enumerable.Range(MIN, MAX - MIN + 1).ToArray();

        protected override bool EqualsCore(BranchLength other)
        {
            return other.Value == Value;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }
    }
}
