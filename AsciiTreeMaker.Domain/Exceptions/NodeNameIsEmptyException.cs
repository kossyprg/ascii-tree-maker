namespace AsciiTreeMaker.Domain.Exceptions
{
    /// <summary>
    /// ノード名が空欄あるいは空白文字のみ
    /// </summary>
    public sealed class NodeNameIsEmptyException : Exception
    {
        /// <summary>
        /// ノード名が空欄あるいは空白文字のみ
        /// </summary>
        public NodeNameIsEmptyException() : base("ノード名を入力してください。")
        {
        }
    }
}
