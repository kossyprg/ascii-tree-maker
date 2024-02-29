using System.Windows.Forms;
using System.Xml;

namespace AsciiTreeMaker
{
    /*
     * ImportXmlFile クラス
     * XMLファイルを読み込み樹形図を構成する機能を有する
     */
    internal static class ImportXmlFile
    {
        private const string NODETEXT_IF_FAILED_TO_LOAD = "(ノードの復元に失敗しました)";

        /// <summary>
        /// 指定されたパスにある XML ファイルを読み込んで樹形図を構成する
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="nodeEditor"></param>
        /// <param name="filepath">XMLファイルのフルパス</param>
        /// <returns></returns>
        public static bool LoadXmlFileAndCreateTreeView(System.Windows.Forms.TreeView treeView, NodeEditor nodeEditor, string filepath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                // XMLファイルを読み込む
                xmlDoc.Load(filepath);
            }
            catch (XmlException ex)
            {
                MessageBox.Show("XMLファイルの読み込み中にエラーが発生しました \n\n詳細\n" + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            // 読み込みが完了するまで再描画しない
            treeView.BeginUpdate();

            nodeEditor.ClearAllNodes();

            // XMLのルート要素から再帰的にTreeViewを構築する
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
            {
                CreateTreeNodeFromXml(nodeEditor, xmlNode, treeView.Nodes);
            }

            treeView.EndUpdate();
            return true;
        }

        /// <summary>
        /// XML のノードから樹形図のノードを生成する
        /// 内部で再帰呼び出しを行う
        /// </summary>
        /// <param name="nodeEditor"></param>
        /// <param name="xmlNode"></param>
        /// <param name="treeNodes"></param>
        /// <returns></returns>
        private static TreeNode CreateTreeNodeFromXml(NodeEditor nodeEditor, XmlNode xmlNode, TreeNodeCollection treeNodes)
        {
            // 属性がおかしい場合、失敗したことを表す文字列を埋め込んで動作は続けるものとする
            string nodeText = (xmlNode.Attributes["Text"] != null) ? xmlNode.Attributes["Text"].Value : NODETEXT_IF_FAILED_TO_LOAD;

            NodeLabel nodeLabel = new NodeLabel(nodeText);
            TreeNode treeNode = nodeEditor.AddNode(treeNodes, nodeLabel);

            // 子ノードがある場合、再帰的に処理する
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                CreateTreeNodeFromXml(nodeEditor, xmlChildNode, treeNode.Nodes);
            }

            return treeNode;
        }
    }
}
