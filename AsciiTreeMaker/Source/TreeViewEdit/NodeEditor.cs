using System;
using System.Windows.Forms;

namespace AsciiTreeMaker
{
    /*
     * NodeEditor クラス
     * 樹形図の編集する機能を実現する
     */
    public class NodeEditor
    {
        private readonly TreeView treeView;
        

        public enum SwapType
        {
            PREVIOUS, // 1個上のノードと交換
            NEXT      // 1個下のノードと交換
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="treeView"></param>
        public NodeEditor(TreeView treeView)
        {
            this.treeView = treeView;
        }

        /// <summary>
        /// TreeView の内容を初期化する
        /// </summary>
        public void InitializeTreeViewContents()
        {
            ClearAllNodes();
            AddNode(treeView.Nodes, NodeLabel.InitialNodeLabel(), true);
            return;
        }

        /// <summary>
        /// 選択されたノードの兄弟ノードを作成する
        /// </summary>
        /// <param name="nodeText">ノードのテキスト</param>
        /// <returns>ノードの追加に成功したら true</returns>
        public bool AddBrotherNode(string nodeText = null)
        {
            NodeLabel nodeLabel = new NodeLabel(nodeText);

            // 兄弟ノードが選択されていないと追加できない
            // Mainform側で例外処理できるようにする
            if (treeView.SelectedNode == null)
            {
                return false;
            }

            TreeNode selectedNode = treeView.SelectedNode;

            if (selectedNode.Parent == null)
            {
                // 親ノードがないなら、最上位層に追加
                AddNode(treeView.Nodes, nodeLabel, true);
            }
            else
            {
                // 親ノードの子ノードとして追加する
                AddNode(selectedNode.Parent.Nodes, nodeLabel, true);
            }
            return true;
        }

        /// <summary>
        /// 選択されたノードの子ノードを作成する
        /// </summary>
        /// <param name="nodeText">ノードのテキスト</param>
        /// <returns>ノードの追加に成功したら true</returns>
        public bool AddChildNode(string nodeText = null)
        {
            NodeLabel nodeLabel = new NodeLabel(nodeText);

            // 親が選択されていないと子を追加できない
            // Mainform側で例外処理できるようにする
            if (treeView.SelectedNode == null)
            {
                return false;
            }

            // 選択されているノードの子ノードを追加する
            AddNode(treeView.SelectedNode.Nodes, nodeLabel, true);

            return true;
        }

        /// <summary>
        /// 選択状態のノードを削除する
        /// </summary>
        /// <returns>削除に成功したら true</returns>
        public bool RemoveSelectedNode()
        {
            // 選択状態のノードがないなら false を返す
            // Mainform側で例外処理できるようにするため
            if (treeView.SelectedNode == null) return false;

            treeView.SelectedNode.Remove();
            return true;
        }

        /// <summary>
        /// 樹形図の全てのノードを削除する
        /// </summary>
        public void ClearAllNodes()
        {
            treeView.Nodes.Clear();
        }

        /// <summary>
        /// nodes の兄弟ノードを作成する
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeLabel"></param>
        /// <param name="isEditing">ユーザがノードを追加する処理を行った場合 true, ファイルを読み込むなどの処理は false</param>
        /// <returns></returns>
        public TreeNode AddNode(in TreeNodeCollection nodes, in NodeLabel nodeLabel, in bool isEditing = false)
        {
            TreeNode node = nodes.Add(nodeLabel.Text);

            // 追加したノードをすぐに編集できるようにする
            // こうすることで、テキストを編集する際にノードをクリックする必要がない
            if (isEditing == true)
            {
                node.EnsureVisible();
                node.BeginEdit();
                treeView.SelectedNode = node;
            }
            return node;
        }

        /// <summary>
        /// 選択したノードとそれと隣接するノードを交換する
        /// </summary>
        /// <param name="swapType">交換するノードの種別</param>
        /// <returns>交換に成功したら true</returns>
        public bool SwapNodes(in SwapType swapType)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            TreeNode targetNode   = null;

            switch (swapType)
            {
                case SwapType.PREVIOUS:
                    targetNode = selectedNode.PrevNode; // 上方向へスワップ
                    break;
                case SwapType.NEXT:
                    targetNode = selectedNode.NextNode; // 下方向へスワップ
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (selectedNode == null || targetNode == null)
            {
                return false;
            }

            TreeNode parentNode = selectedNode.Parent;
            treeView.BeginUpdate();

            if (parentNode == null)
            {
                // 選択したノードが最上位のノードであるとき
                int x = treeView.Nodes.IndexOf(selectedNode);
                int y = treeView.Nodes.IndexOf(targetNode);

                treeView.Nodes.Remove(selectedNode);
                treeView.Nodes.Insert(y, selectedNode);

                treeView.Nodes.Remove(targetNode);
                treeView.Nodes.Insert(x, targetNode);
            }
            else
            {
                int x = parentNode.Nodes.IndexOf(selectedNode);
                int y = parentNode.Nodes.IndexOf(targetNode);

                parentNode.Nodes.Remove(selectedNode);
                parentNode.Nodes.Insert(y, selectedNode);

                parentNode.Nodes.Remove(targetNode);
                parentNode.Nodes.Insert(x, targetNode);
            }

            // 選択していたノードを追跡する
            treeView.SelectedNode = selectedNode;

            treeView.EndUpdate();

            return true;
        }
    }
}