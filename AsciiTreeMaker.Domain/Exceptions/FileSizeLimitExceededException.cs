namespace AsciiTreeMaker.Domain.Exceptions
{
    /// <summary>
    /// ファイルサイズが上限を超えている
    /// </summary>
    public sealed class FileSizeLimitExceededException : Exception
    {
        /// <summary>
        /// ファイルサイズが上限を超えている
        /// </summary>
        public FileSizeLimitExceededException(string message) : base(message)
        {
        }
    }
}
