using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Domain.Entities
{
    /// <summary>
    /// ユーザ設定エンティティ
    /// </summary>
    public sealed class UserPreferenceEntity
    {
        /// <summary>
        /// 枝の長さ
        /// </summary>
        public BranchLength BranchLength { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="branchLength">枝の長さ</param>
        public UserPreferenceEntity(BranchLength branchLength)
        {
            BranchLength = branchLength;
        }

        /// <summary>
        /// ユーザ設定の初期値を返す
        /// </summary>
        /// <returns>ユーザ設定の初期値</returns>
        public static UserPreferenceEntity GetInitialUserPreferenceEntity()
        {
            return new UserPreferenceEntity(BranchLength.Default);
        }
    }
}
