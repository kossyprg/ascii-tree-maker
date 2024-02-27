using System.Text;
using System.Windows.Forms;

namespace AsciiTreeMaker
{
    /*
     * TreeConverter クラス
     * 樹形図をテキスト形式へ変換する機能を実現する
     */
    internal class TreeConverter
    {
        private readonly StringBuilder TextFormatTree = new StringBuilder();
        private readonly System.Windows.Forms.TreeView treeView;
        private TextFormatTreeDesigner textFormatTreeDesigner;

        // lastChildFlag が 64 bitなので、level = 0,1,...,63 までの階層ならテキスト変換が正常に行われる
        private readonly int MAX_LEVEL = 63;
        private bool _over_level = false;

        /// <summary>
        /// TreeConverter のコンストラクタ
        /// </summary>
        public TreeConverter(in TreeView treeView, in TextFormatTreeDesigner textFormatTreeDesigner)
        {
            this.treeView = treeView;
            this.textFormatTreeDesigner = textFormatTreeDesigner;
        }

        /// <summary>
        /// 作成した樹形図をテキスト形式へ変換する
        /// </summary>
        /// <param name="textFormatTreeDesigner">テキスト形式の樹形図のデザイン</param>
        /// <returns></returns>
        public string ConvertTreeToText(in TextFormatTreeDesigner textFormatTreeDesigner)
        {
            this.textFormatTreeDesigner = textFormatTreeDesigner;

            // テキスト変換後の樹形図を一度削除する
            TextFormatTree.Clear();

            // 階層の上限値を超えていないかを確認する
            _over_level = false;
            ConvertNodesToText(treeView.Nodes); // 0 レベルからスタート

            // 許容できる階層を超えた場合はメッセージを出す
            // 変換を諦めたところ以外はそのまま出力する
            if (_over_level == true)
            {
                string message = string.Format("許容できる階層の数({0})を超えたため、一部のノードが変換されません", MAX_LEVEL + 1);
                MessageBox.Show(message);
            }

            return TextFormatTree.ToString();
        }

        /// <summary>
        /// nodes 以下のノードをテキスト形式に変換する
        /// 内部で再帰的に処理している
        /// 最上位のノードを渡せば、木全体を変換することになる
        /// </summary>
        /// <param name="nodes">ノード</param>
        /// <param name="lastChildFlag">親ノードからの伸ばし棒を表示するか否かを管理する値</param>
        private void ConvertNodesToText(TreeNodeCollection nodes, ulong lastChildFlag = 0UL)
        {
            // ノードの階層レベルを取得する
            // level = 0 からスタート
            int level = nodes[0].Level;

            // 許容できる階層を超えた場合は変換を断念する
            if (level > MAX_LEVEL)
            {
                _over_level = true;
                return;
            }

            // nodes の兄弟ノードの個数
            int n_brothers = nodes.Count;

            for (int i = 0; i < n_brothers; i++)
            {
                // nodes[i] が nodes の最後の兄弟ノードなら、親ノードからの伸ばし棒は不要
                // 伸ばし棒が不要なレベルに 1 を立てる
                if (i == n_brothers - 1 && level >= 1)
                {
                    lastChildFlag |= (1UL << (level - 1));
                }

                // nodes[i] をテキストへ変換
                ConvertOneNodeToText(nodes[i].Text, level, i, n_brothers, lastChildFlag);

                // 子ノードがあるなら、それらを変換
                if (nodes[i].Nodes.Count > 0)
                {
                    ConvertNodesToText(nodes[i].Nodes, lastChildFlag);
                }
            }
        }

        /// <summary>
        /// 特定のノードをテキストへ変換する
        /// </summary>
        /// <param name="nodeText">ノードの文字列</param>
        /// <param name="level">ノードのレベル(level >= 0)</param>
        /// <param name="cnt">兄弟ノードのうち何個目のノードか(0 <= cnt <= n_brothers - 1)</param>
        /// <param name="n_brothers">兄弟ノードの個数</param>
        /// <param name="lastChildFlag">親ノードからの伸ばし棒を表示するか否かを管理する値</param>
        private void ConvertOneNodeToText(string nodeText, int level, int cnt, int n_brothers, ulong lastChildFlag)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < level - 1; i++)
            {
                // 親ノードからの伸ばし棒を表示するかを判定する
                if ((lastChildFlag & (1UL << i)) == 0)
                {
                    // 親ノードからの伸ばし棒を表示
                    sb.AppendFormat(textFormatTreeDesigner.VerticalLine);
                }
                else
                {
                    sb.AppendFormat(textFormatTreeDesigner.Blank);
                }
            }

            if (level > 0)
            {
                if (cnt == n_brothers - 1)
                {
                    // 最後の兄弟ノード
                    sb.AppendFormat(textFormatTreeDesigner.LastNodeBranch);
                }
                else
                {
                    sb.AppendFormat(textFormatTreeDesigner.IntermediateNodeBranch);
                }
            }

            sb.Append(nodeText);

            // 末尾に改行を付けながら、TextFormatTree へ格納していく
            TextFormatTree.AppendLine(sb.ToString());

            // デバッグ用
            //Console.WriteLine(sb.ToString());
        }
    }
}
