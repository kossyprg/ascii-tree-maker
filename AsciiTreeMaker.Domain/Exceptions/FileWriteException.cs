namespace AsciiTreeMaker.Domain.Exceptions
{
    /// <summary>
    /// ファイルの書き込みに失敗
    /// </summary>
    public sealed class FileWriteException : Exception
    {
        /// <summary>
        /// ファイルの書き込みに失敗
        /// </summary>
        /// <param name="message">メッセージ</param>
        public FileWriteException(string message) : base(message)
        {
        }

        /// <summary>
        /// ファイルの書き込みに失敗
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">内部例外</param>
        public FileWriteException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
