namespace AsciiTreeMaker.Domain.Repositories
{
    /// <summary>
    /// ファイルI/Oを行うためのインタフェース
    /// </summary>
    public interface ITreeFileIO
    {
        /// <summary>
        /// 木構造データを読み込む
        /// </summary>
        /// <param name="path">外部ソースの読込元パス</param>
        /// <returns>木構造データ</returns>
        IReadOnlyList<ITreeNode> LoadFrom(string path);

        /// <summary>
        /// 木構造データを保存する
        /// </summary>
        /// <param name="path">外部ソースの保存先パス</param>
        /// <param name="roots">木構造データ</param>
        void SaveTo(string path, IReadOnlyList<ITreeNode> roots);
    }
}
