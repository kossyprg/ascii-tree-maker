using AsciiTreeMaker.Domain.Repositories;
using System.Text;

namespace AsciiTreeMaker.Domain.Logics
{
    /// <summary>
    /// 木構造データをアスキー文字による樹形図として文字列に変換するビルダー
    /// </summary>
    public static class AsciiTreeBuilder
    {
        /// <summary>
        /// 指定したルートノード以下の木構造を、
        /// アスキー文字で表現された樹形図として構築する
        /// </summary>
        /// <param name="root">ルートとなるノード</param>
        /// <param name="branchLength">枝の長さ</param>
        /// <returns>木構造を表すアスキー文字列</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Build(ITreeNode root, int branchLength)
        {
            if (branchLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(branchLength), "branchLength must be >= 0.");
            }

            var sb = new StringBuilder();

            // ルートは素で出す
            sb.AppendLine(root.Name.Value);

            // 子を再帰で出す
            var children = root.Children;
            for (int i = 0; i < children.Count; i++)
            {
                bool isLast = i == children.Count - 1;
                AppendNode(sb, children[i], prefix: "", isLast: isLast, branchLength: branchLength);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 指定したノードをアスキー文字で出力し、
        /// その子ノードを再帰的に処理する。
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="node">出力対象のノード</param>
        /// <param name="prefix">現在の階層までのインデントおよび縦線を表す文字列</param>
        /// <param name="isLast">このノードが同階層の最後のノードならTrue</param>
        /// <param name="branchLength">枝の長さ</param>
        private static void AppendNode(
            StringBuilder sb,
            ITreeNode node,
            string prefix,
            bool isLast,
            int branchLength)
        {
            sb.Append(prefix);
            sb.Append(isLast ? "└" : "├");
            sb.Append(new string('─', branchLength));
            sb.Append(' ');
            sb.AppendLine(node.Name.Value);

            // 次の階層用 prefix：
            // 兄弟が続くなら "│"、最後なら " "
            // その後に空白（L 個 + 1個）を付ける（子の表示位置を揃える）
            var childPrefix = prefix
                + (isLast ? " " : "│")
                + new string(' ', branchLength + 1);

            var children = node.Children;
            for (int i = 0; i < children.Count; i++)
            {
                bool childIsLast = i == children.Count - 1;
                AppendNode(sb, children[i], childPrefix, childIsLast, branchLength);
            }
        }
    }
}
