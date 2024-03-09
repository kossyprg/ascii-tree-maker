using System;
using System.Drawing;
using System.Windows.Forms;
using AsciiTreeMaker.Source;

namespace AsciiTreeMaker
{
    /*
     * Mainform クラス
     */
    public partial class Mainform : Form
    {
        private readonly TreeConverter treeConverter;
        private readonly NodeEditor nodeEditor;
        private readonly FileManager fileManager;
        private readonly TextFormatTreeDesigner textFormatTreeDesigner;
        private readonly TextFormatTreeBranchLength branchLength;

        /// <summary>
        /// Mainform コンストラクタ
        /// </summary>
        public Mainform()
        {
            InitializeComponent();

            nodeEditor = new NodeEditor(treeView);

            EditingFile editingFile = new EditingFile();
            editingFile.EditFileStatusChanged += UpdateEditingFileName;
            fileManager = new FileManager(treeView, editingFile);
            fileManager.InitializeFileState();

            branchLength = TextFormatTreeBranchLength.DefaultTextFormatTreeBranchLength();
            textFormatTreeDesigner = new TextFormatTreeDesigner(branchLength);
            treeConverter = new TreeConverter(treeView, textFormatTreeDesigner);
                        
            this.treeView.BeforeLabelEdit += new NodeLabelEditEventHandler(TreeView_BeforeLabelEdit);
            this.treeView.AfterLabelEdit += new NodeLabelEditEventHandler(TreeView_AfterLabelEdit);

            CreateNewTreeView();
        }

        /// <summary>
        /// ユーザがノードのテキストを編集する前に実行する処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // ラベル編集中に toolStrip のボタンを押下すると挙動がおかしくなるから無効にする
            toolStrip.Enabled = false;

            // バグらないけど意図した動きではないので無効にする
            menuStrip.Enabled = false;
        }

        /// <summary>
        /// ユーザがノードのテキストを編集した後に実行する処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // 復活させる
            toolStrip.Enabled = true;
            menuStrip.Enabled = true;

            // 編集後のテキスト e.Label が空なら編集を無効にする
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            fileManager.TreeViewHasChanged();
        }

        /// <summary>
        /// 樹形図が空なら true を返す
        /// </summary>
        /// <returns></returns>
        public bool TreeViewIsEmpty()
        {
            return treeView.Nodes.Count == 0;
        }

        /// <summary>
        /// フォームのタイトル横に表示するファイル名を更新する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEditingFileName(object sender, EventArgs e)
        {
            if (fileManager.editingFile.IsSaved())
            {
                this.Text = String.Format("{0} - [ {1} ]", AppBaseInfo.APP_NAME, fileManager.editingFile.GetName());
            }
            else
            {
                // 未保存の編集内容がある場合
                this.Text = String.Format("{0} - [ {1} ]*", AppBaseInfo.APP_NAME, fileManager.editingFile.GetName());
            }
        }

        /// <summary>
        /// 子ノードを追加するボタンを押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddChildNode_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool success = nodeEditor.AddChildNode();
            if (success == false)
            {
                IWarning warning = new NoNodeSelected();
                warning.ShowWarningMessageBox();
                return;
            }

            fileManager.TreeViewHasChanged();
        }

        /// <summary>
        /// 兄弟ノードを追加するボタンを押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBrotherNode_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool success = nodeEditor.AddBrotherNode();
            if (success == false)
            {
                IWarning warning = new NoNodeSelected();
                warning.ShowWarningMessageBox();
                return;
            }

            fileManager.TreeViewHasChanged();
        }

        /// <summary>
        /// 選択したノードを削除するボタンを押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveSelectedNode_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool success = nodeEditor.RemoveSelectedNode();
            if (success == false)
            {
                IWarning warning = new NoNodeSelected();
                warning.ShowWarningMessageBox();
                return;
            }

            // 樹形図が空になったら、実質的に初期化処理を行う
            if (TreeViewIsEmpty())
            {
                nodeEditor.InitializeTreeViewContents();
            }

            fileManager.TreeViewHasChanged();
        }

        /// <summary>
        /// 樹形図をテキストへ変換するボタンを押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertTreeViewToText_ToolStripButton_Click(object sender, EventArgs e)
        {
            // 樹形図が空なら例外を出す
            if (TreeViewIsEmpty())
            {
                IWarning warning = new TreeViewIsEmpty();
                warning.ShowWarningMessageBox();
                return;
            }

            // テキスト変換
            string textFormatTree = treeConverter.ConvertTreeToText(textFormatTreeDesigner);

            // クリップボードへ保存
            Clipboard.SetText(textFormatTree);

            // 処理後のメッセージ
            MessageBox.Show("クリップボードにコピーしました",
                        AppBaseInfo.APP_NAME,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// ファイルタブの「新規作成」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewTreeView_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ショートカットキーを押下したときも呼ばれることに注意
            // コンポーネントが無効なら実行しない
            if (!menuStrip.Enabled)
            {
                return;
            }
            CreateNewTreeView();
        }

        /// <summary>
        /// 樹形図を新規作成する
        /// </summary>
        private void CreateNewTreeView()
        {
            // 未保存の編集内容がある時だけユーザに確認をとる(短絡評価)
            if (fileManager.editingFile.IsSaved() || ConfirmClearTreeView())
            {
                nodeEditor.InitializeTreeViewContents();
                fileManager.InitializeFileState();
            }
        }

        /// <summary>
        /// ファイルタブの「開く」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenXMLFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!menuStrip.Enabled)
            {
                return;
            }
            LoadXmlFile();
        }

        /// <summary>
        /// XMLファイルを読み込んで樹形図に反映させる
        /// </summary>
        private void LoadXmlFile()
        {
            // 未保存の編集内容がある時だけユーザに確認をとる(短絡評価)
            if (fileManager.editingFile.IsSaved() || ConfirmClearTreeView())
            {
                fileManager.AskAndLoadFile();
            }
        }

        /// <summary>
        /// ファイルタブの「フォルダから作成」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFromFolder_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 未保存の編集内容がある時だけユーザに確認をとる(短絡評価)
            if (fileManager.editingFile.IsSaved() || ConfirmClearTreeView())
            {
                fileManager.AskRootFolderAndCreateDirectoryStructureTreeView();
            }
        }

        /// <summary>
        /// ファイルタブの「保存」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveXMLFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!menuStrip.Enabled)
            {
                return;
            }
            SaveXmlFile();
        }

        /// <summary>
        /// 現在の樹形図をファイルに上書き保存する
        /// 保存したことのないファイルなら名前を付けて保存させる
        /// </summary>
        private void SaveXmlFile()
        {
            // 上書き保存を試す
            bool success = fileManager.Save();
            if (!success)
            {
                // 名前を付けて保存を試す
                SaveAsXMLFile();
            }
        }

        /// <summary>
        /// ファイルタブの「名前を付けて保存」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsXMLFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!menuStrip.Enabled)
            {
                return;
            }
            SaveAsXMLFile();
        }

        /// <summary>
        /// 名前を付けた上でファイルを保存する
        /// </summary>
        private void SaveAsXMLFile()
        {
            // 作成した樹形図をXML形式で保存する
            fileManager.AskAndSaveFile();
        }
        
        /// <summary>
        /// 現在の樹形図を破棄してよいか確認する
        /// </summary>
        /// <returns>破棄してよいなら true</returns>
        private bool ConfirmClearTreeView()
        {
            DialogResult result = MessageBox.Show("保存していない樹形図は破棄されますが、よろしいですか？",
                AppBaseInfo.APP_NAME,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// 「下へ移動」のボタンを押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwapNextNode_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (nodeEditor.SwapNodes(NodeEditor.SwapType.NEXT))
            {
                fileManager.TreeViewHasChanged();
            }
        }

        /// <summary>
        /// 「上へ移動」のボタンを押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwapPreviousNode_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (nodeEditor.SwapNodes(NodeEditor.SwapType.PREVIOUS))
            {
                fileManager.TreeViewHasChanged();
            }
        }

        /// <summary>
        /// ファイルタブの「終了」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!menuStrip.Enabled)
            {
                return;
            }

            // Mainform_FormClosing が呼び出される
            Application.Exit();
        }

        /// <summary>
        /// Mainform が閉じられるときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool success = ConfirmBeforeExitApp();
            if (!success)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// アプリを終了してよいかを確認する
        /// </summary>
        private bool ConfirmBeforeExitApp()
        {
            // 未保存の編集内容があるなら、破棄してよいか確認する
            if (!fileManager.editingFile.IsSaved() && !ConfirmClearTreeView())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ファイルタブの「設定」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mainform を無効化
            this.Enabled = false;

            using (SettingsForm settingsForm = new SettingsForm(branchLength))
            {
                // Mainform の近くで開くようにする
                settingsForm.StartPosition = FormStartPosition.Manual;
                const int OFFSET_PIXEL_X = 50;
                const int OFFSET_PIXEL_Y = 50;
                settingsForm.Location = new Point(this.Location.X + OFFSET_PIXEL_X, this.Location.Y + OFFSET_PIXEL_Y);

                settingsForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SettingsForm_FormClosed);
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    branchLength.Update(settingsForm.UpdatedBranchLength);
                    textFormatTreeDesigner.UpdateBranchLength(branchLength);
                }
            }
        }

        /// <summary>
        /// 設定ウィンドウが閉じた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // MainFormを有効化
            this.Enabled = true;
        }

        /// <summary>
        /// 「すべて展開」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandTreeView_ToolStripButton_Click(object sender, EventArgs e)
        {
            this.treeView.BeginUpdate();
            this.treeView.ExpandAll();
            this.treeView.EndUpdate();
        }

        /// <summary>
        /// 「すべて折りたたむ」を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseTreeView_ToolStripButton_Click(object sender, EventArgs e)
        {
            this.treeView.BeginUpdate();
            this.treeView.CollapseAll();
            this.treeView.EndUpdate();
        }

        /// <summary>
        /// キーが押されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeView.SelectedNode == null)
            {
                return;
            }

            // F2キーで即時入力モードにする
            if (e.KeyCode == Keys.F2)
            {
                treeView.SelectedNode.BeginEdit();
            }
        }
    }
}
