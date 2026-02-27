using Prism.Mvvm;

namespace AsciiTreeMaker.UI.Models
{
    /// <summary>
    /// ドキュメント状態を保持する
    /// </summary>
    public sealed class TreeDocument : BindableBase
    {
        /// <summary>
        /// ファイルパス
        /// </summary>
        private string _filePath = string.Empty;

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        /// <summary>
        /// 未保存の変更があればTrue
        /// </summary>
        private bool _isDirty;

        /// <summary>
        /// 未保存の変更があればTrue
        /// </summary>
        public bool IsDirty
        {
            get => _isDirty;
            set => SetProperty(ref _isDirty, value);
        }

        /// <summary>
        /// ファイルが作成済みならTrue。
        /// Trueの時は上書き保存が可能になる。
        /// </summary>
        public bool HasFile
        {
            get => !string.IsNullOrWhiteSpace(FilePath);
        }
    }

}
