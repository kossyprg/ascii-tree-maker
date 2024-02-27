namespace AsciiTreeMaker
{
    public class NodeLabel
    {
        private const string INITIAL_NODE_LABEL = "テキストを入力";
        public string Text { get; private set; }

        public NodeLabel(in string nodeLabel)
        {
            if (string.IsNullOrEmpty(nodeLabel))
            {
                Text = INITIAL_NODE_LABEL;
                return;
            }

            Text = nodeLabel;
        }

        public static NodeLabel InitialNodeLabel()
        {
            return new NodeLabel(INITIAL_NODE_LABEL);
        }
    }
}
