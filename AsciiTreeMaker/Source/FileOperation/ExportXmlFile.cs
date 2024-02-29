using System.Windows.Forms;
using System.Xml;

namespace AsciiTreeMaker
{
    /*
     * ExportXmlFile クラス
     * XML形式で出力する機能を有する
     */
    internal static class ExportXmlFile
    {
        /// <summary>
        /// 樹形図をXML形式で出力する
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="filepath">完全パス</param>
        public static void SaveTreeViewAsXmlFile(System.Windows.Forms.TreeView treeView, string filepath)
        {
            XmlDocument exportXmlDocument = new XmlDocument();

            // XMLヘッダ出力(バージョン 1.0, エンコード UTF-8)
            XmlDeclaration xmlDeclaration = exportXmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            exportXmlDocument.AppendChild(xmlDeclaration);

            XmlElement xmlRoot = exportXmlDocument.CreateElement("TreeViewData"); // ルート要素を作成

            // 最上位のノードを追加していく
            // 内部で深さ優先探索する
            foreach (TreeNode rootNode in treeView.Nodes)
            {
                XmlElement xmlNode = CreateXmlNode(exportXmlDocument, rootNode);
                xmlRoot.AppendChild(xmlNode);
            }

            // ルート要素をドキュメントに追加
            exportXmlDocument.AppendChild(xmlRoot);

            // ファイルに出力
            exportXmlDocument.Save(filepath);
        }

        /// <summary>
        /// TreeNodeをXML要素に変換する
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        private static XmlElement CreateXmlNode(XmlDocument xmlDoc, TreeNode treeNode)
        {
            XmlElement xmlNode = xmlDoc.CreateElement("Node");
            xmlNode.SetAttribute("Text", treeNode.Text);

            // 子ノードがある場合、再帰的に処理する
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                XmlElement xmlChildNode = CreateXmlNode(xmlDoc, childNode);
                xmlNode.AppendChild(xmlChildNode);
            }

            return xmlNode;
        }
    }
}
