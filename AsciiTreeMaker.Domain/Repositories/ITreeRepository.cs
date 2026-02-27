namespace AsciiTreeMaker.Domain.Repositories
{
    /// <summary>
    /// 木構造データリポジトリー
    /// </summary>
    public interface ITreeRepository
    {
        /// <summary>
        /// 木構造のルートノード群を読み込む
        /// </summary>
        /// <returns>木構造のルートノード群</returns>
        IReadOnlyList<ITreeNode> LoadTree();

        /// <summary>
        /// 木構造データを保存する
        /// </summary>
        /// <param name="roots">木構造のルートノード群</param>
        void SaveTree(IReadOnlyList<ITreeNode> roots);
    }
}
