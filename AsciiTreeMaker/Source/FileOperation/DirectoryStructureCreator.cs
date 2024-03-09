
using System.IO;
using System.Windows.Forms;

namespace AsciiTreeMaker
{
    /*
     * DirectoryStructureCreator クラス
     * ディレクトリ構成を表すツリービューを生成する機能を有する
     */
    internal static class DirectoryStructureCreator
    {
        /// <summary>
        /// ディレクトリ構成を表すツリービューを生成する
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="nodeEditor"></param>
        /// <param name="rootFolderPath">起点となるフォルダのフルパス</param>
        /// <returns></returns>
        public static bool CreateDirectoryStructureTreeView(TreeView treeView, NodeEditor nodeEditor, string rootFolderPath)
        {
            // 読み込みが完了するまで再描画しない
            treeView.BeginUpdate();

            nodeEditor.ClearAllNodes();

            // 樹形図の生成に失敗したら、中途半端な樹形図を作ってもしょうがないので初期化する
            if (!CreateTreeNodeFromFolder(rootFolderPath, treeView.Nodes, nodeEditor))
            {
                nodeEditor.InitializeTreeViewContents();
                treeView.EndUpdate();
                return false;
            }

            treeView.EndUpdate();
            return true;
        }

        /// <summary>
        /// フォルダからツリーノードを作成する
        /// 内部で再帰呼び出しを行う
        /// </summary>
        /// <param name="rootFolderPath">起点となるフォルダのフルパス</param>
        /// <param name="treeNodes"></param>
        /// <param name="nodeEditor"></param>
        /// <returns>樹形図の生成に失敗したら false</returns>
        private static bool CreateTreeNodeFromFolder(string rootFolderPath, TreeNodeCollection treeNodes, NodeEditor nodeEditor)
        {
            NodeLabel rootFolderLabel = new NodeLabel(Path.GetFileName(rootFolderPath));
            TreeNode node = nodeEditor.AddNode(treeNodes, rootFolderLabel);

            string[] folders;
            string[] files;

            try
            {
                folders = Directory.GetDirectories(rootFolderPath);
                files = Directory.GetFiles(rootFolderPath);
            }
            catch (System.UnauthorizedAccessException)
            {
                // アクセス権がないファイルを読み込もうとするとここに来る
                // Cドライブ直下のフォルダを選択した際に発生しやすい
                // 中途半端な樹形図を作ってもしょうがないので、生成を断念する
                MessageBox.Show("ファイルのアクセスに失敗しました\n",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            foreach (string f in folders)
            {
                if (!CreateTreeNodeFromFolder(f, node.Nodes, nodeEditor))
                {
                    return false;
                }
            }

            foreach (string file in files)
            {
                NodeLabel fileNodeLabel = new NodeLabel(Path.GetFileName(file));
                nodeEditor.AddNode(node.Nodes, fileNodeLabel);
            }

            return true;
        }
    }
}
