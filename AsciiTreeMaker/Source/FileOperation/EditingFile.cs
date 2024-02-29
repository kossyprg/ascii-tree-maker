using System;
using System.IO;

namespace AsciiTreeMaker
{
    /*
     * EditingFile クラス
     * 編集中のファイルの情報を管理する
     */
    internal class EditingFile
    {
        private const string INITIAL_FILE_NAME = "Untitled.xml";
        private const string EMPTY_FILE_PATH = "";

        private string _filepath;
        private bool _isSaved;
        
        public event EventHandler EditFileStatusChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="editingFilepath"></param>
        public EditingFile(in string editingFilepath = EMPTY_FILE_PATH, in bool isSaved = true)
        {
            _isSaved = isSaved;
            UpdatePath(editingFilepath);
        }

        /// <summary>
        /// 編集中ファイルの状態を初期化する
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            UpdatePath(EMPTY_FILE_PATH);
            UpdateSaveState(true);
        }

        /// <summary>
        /// ファイルパスを更新する
        /// </summary>
        /// <param name="filepath"></param>
        public void UpdatePath(in string filepath)
        {
            _filepath = filepath;
            SaveStateOrFilePathChanged();
        }

        /// <summary>
        /// ファイルパスを取得する
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            if (this.IsNotDefined())
            {
                // 未定義の状態でパスは取得できない
                throw new InvalidOperationException();
            }
            return _filepath;
        }

        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if (this.IsNotDefined())
            {
                return INITIAL_FILE_NAME;
            }

            return Path.GetFileName(_filepath);
        }

        /// <summary>
        /// ファイルの保存状態を更新する
        /// </summary>
        /// <param name="isSaved"></param>
        public void UpdateSaveState(bool isSaved)
        {
            if (_isSaved != isSaved)
            {
                _isSaved = isSaved;
                SaveStateOrFilePathChanged();
            }
        }

        /// <summary>
        /// 保存されていれば true
        /// </summary>
        /// <returns></returns>
        public bool IsSaved()
        {
            return _isSaved;
        }

        /// <summary>
        /// ファイルパスが未定義なら true
        /// </summary>
        /// <returns></returns>
        public bool IsNotDefined()
        {
            return string.IsNullOrEmpty(_filepath);
        }

        /// <summary>
        /// 変更通知のメソッド
        /// </summary>
        protected virtual void SaveStateOrFilePathChanged()
        {
            // イベントを発生させる
            EditFileStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
