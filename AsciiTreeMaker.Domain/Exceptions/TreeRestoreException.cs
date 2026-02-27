
namespace AsciiTreeMaker.Domain.Exceptions
{
    /// <summary>
    /// 木構造データの復元に失敗
    /// </summary>
    public sealed class TreeRestoreException : Exception
    {
        /// <summary>
        /// 詳細情報
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// 木構造データの復元に失敗
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="detail">(任意) 詳細情報</param>
        /// <param name="innerException">内部例外</param>
        public TreeRestoreException(string message, string detail, Exception? innerException = null)
            : base(message, innerException)
        {
            Detail = detail;
        }
    }
}
