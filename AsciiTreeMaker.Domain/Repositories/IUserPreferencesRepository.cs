using AsciiTreeMaker.Domain.Entities;

namespace AsciiTreeMaker.Domain.Repositories
{
    /// <summary>
    /// ユーザ設定リポジトリ
    /// </summary>
    public interface IUserPreferencesRepository
    {
        /// <summary>
        /// ユーザ設定を読み込む
        /// </summary>
        /// <returns></returns>
        UserPreferenceEntity Load();

        /// <summary>
        /// ユーザ設定を保存する
        /// </summary>
        /// <param name="preferences">ユーザ設定</param>
        void Save(UserPreferenceEntity preferences);
    }
}
