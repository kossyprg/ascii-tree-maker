namespace AsciiTreeMaker.Domain
{
    /// <summary>
    /// Sharedクラス
    /// 起動時に決まってそれ以降覚えておきたいパラメータなどを置いておく
    /// </summary>
    public static class Shared
    {
        /// <summary>
        /// FakeのときTrue
        /// </summary>
        public static bool IsFake { get; private set; }

        /// <summary>
        /// パラメータを設定する
        /// </summary>
        /// <param name="settings"></param>
        public static void Initialize(AppSettings settings)
        {
            IsFake = settings.IsFake;
        }
    }

    /// <summary>
    /// appsettings.json から読み込むデータ
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// FakeのときTrue
        /// </summary>
        public bool IsFake { get; set; } = false;
    }
}
