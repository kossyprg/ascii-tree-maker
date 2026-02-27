namespace AsciiTreeMaker.Domain.Exceptions
{
    /// <summary>
    /// サポートされていないファイル形式
    /// </summary>
    public sealed class UnsupportedFileFormatException : Exception
    {
        /// <summary>
        /// サポートされていないファイル形式
        /// </summary>
        /// <param name="message">メッセージ</param>
        public UnsupportedFileFormatException(string message) : base(message)
        {
        }
    }
}
