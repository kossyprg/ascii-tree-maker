
using System.IO;
using System.Windows.Forms;

namespace AsciiTreeMaker
{
    /*
     * LoadFolder クラス
     * フォルダを読み込んでツリービューを構成する機能を有する
     */
    internal static class LoadFolder
    {
        /// <summary>
        /// 指定されたパスにあるフォルダを読み込んでツリービューを構成する
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="nodeEditor"></param>
        /// <param name="folderPath">フォルダのフルパス</param>
        /// <returns></returns>
        public static bool LoadFolderAndCreateTreeView(TreeView treeView, NodeEditor nodeEditor, string folderPath)
        {
            // 読み込みが完了するまで再描画しない
            treeView.BeginUpdate();

            nodeEditor.ClearAllNodes();

            CreateTreeNodeFromFolder(folderPath, treeView.Nodes);

            treeView.EndUpdate();
            return true;
        }

        /// <summary>
        /// フォルダからツリーノードを作成する
        /// 内部で再帰呼び出しを行う
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="treeNodes"></param>
        private static void CreateTreeNodeFromFolder(string folder, TreeNodeCollection treeNodes)
        {
            string name = Path.GetFileName(folder);
            TreeNode node = new TreeNode(name);
            treeNodes.Add(node);

            string[] folders = Directory.GetDirectories(folder);
            string[] files = Directory.GetFiles(folder);

            foreach (string f in folders)
            {
                CreateTreeNodeFromFolder(f, node.Nodes);
            }

            foreach (string file in files)
            {
                TreeNode n = new TreeNode(Path.GetFileName(file));
                node.Nodes.Add(n);
            }
        }
    }
}
