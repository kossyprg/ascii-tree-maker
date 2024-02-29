namespace AsciiTreeMaker
{
    /*
     * NodeLabel クラス
     * 各ノードのラベル
     */
    public class NodeLabel
    {
        private const string INITIAL_NODE_LABEL = "テキストを入力";
        public string Text { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nodeLabel">ラベルのテキスト</param>
        public NodeLabel(in string nodeLabel)
        {
            if (string.IsNullOrEmpty(nodeLabel))
            {
                Text = INITIAL_NODE_LABEL;
                return;
            }

            Text = nodeLabel;
        }

        /// <summary>
        /// 生成時のラベル
        /// </summary>
        /// <returns></returns>
        public static NodeLabel InitialNodeLabel()
        {
            return new NodeLabel(INITIAL_NODE_LABEL);
        }
    }
}
