using System.Windows.Forms;

namespace AsciiTreeMaker
{
    /*
     * FileManager クラス
     * ファイルの保存状態を監視し、
     * XMLファイルの読み込み、書き出し命令を出す
     */
    internal class FileManager
    {
        private readonly NodeEditor nodeEditor;
        private readonly System.Windows.Forms.TreeView treeView;
        public EditingFile editingFile;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="editingFile"></param>
        public FileManager(System.Windows.Forms.TreeView treeView, EditingFile editingFile)
        {
            nodeEditor = new NodeEditor(treeView);
            this.treeView = treeView;
            this.editingFile = editingFile;
        }

        /// <summary>
        /// 指定したファイルの保存を行う
        /// 指定がない場合は上書き保存を実行する
        /// </summary>
        /// <param name="path"></param>
        /// <returns>保存に成功したら true</returns>
        public bool Save(string path = null)
        {
            if (path == null && editingFile.IsNotDefined())
            {
                // 上書き保存できない
                // Mainform側で名前を付けて保存を実行させる
                return false;
            }

            if (path == null)
            {
                // 上書き保存
                path = editingFile.GetPath();
                ExportXmlFile.SaveTreeViewAsXmlFile(treeView, path);
                editingFile.UpdateSaveState(true);
            } 
            else
            {
                // path として保存
                ExportXmlFile.SaveTreeViewAsXmlFile(treeView, path);
                editingFile.UpdatePath(path);
                editingFile.UpdateSaveState(true);
            }

            return true;
        }

        /// <summary>
        /// 初期状態に戻す
        /// 新規作成時に実行することを想定
        /// </summary>
        public void InitializeFileState()
        {
            editingFile.Initialize();
        }

        /// <summary>
        /// ユーザにロードするファイルを尋ねて読み込む
        /// </summary>
        /// <returns></returns>
        public void AskAndLoadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "ファイルを選択してください";
            openFileDialog.Filter = "XMLファイル|*.xml|すべてのファイル|*.*";
            openFileDialog.FilterIndex = 0;

            // ダイアログを表示して選択されたファイルを開く
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filepath = openFileDialog.FileName;
            if (!ImportXmlFile.LoadXmlFileAndCreateTreeView(treeView, nodeEditor, filepath))
            {
                return;
            }

            editingFile.UpdatePath(filepath);
            editingFile.UpdateSaveState(true);

            return;
        }

        /// <summary>
        /// ユーザにフォルダを尋ねて読み込む
        /// </summary>
        public void AskAndCreateFromFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.Description = "フォルダを選択してください";

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string folderpath = folderBrowserDialog.SelectedPath;
            LoadFolder.LoadFolderAndCreateTreeView(treeView, nodeEditor, folderpath);

            editingFile.UpdatePath(string.Empty);
            editingFile.UpdateSaveState(false);
        }

        /// <summary>
        /// ユーザにファイル名を尋ねて書き出す
        /// </summary>
        /// <returns></returns>
        public bool AskAndSaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "リストファイルを選択してください";
            saveFileDialog.FileName = "*.xml";
            saveFileDialog.Filter = "XMLファイル|*.xml|すべてのファイル|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.DefaultExt = "XML";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            string filepath = saveFileDialog.FileName;
            Save(filepath);

            return true;
        }

        /// <summary>
        /// 樹形図に変更が加わった際のファイル側の処理
        /// </summary>
        public void TreeViewHasChanged()
        {
            bool isSaved = false;
            editingFile.UpdateSaveState(isSaved);
        }
    }
}
