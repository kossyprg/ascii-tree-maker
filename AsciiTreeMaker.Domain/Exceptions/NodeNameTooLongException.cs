using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Domain.Exceptions
{
    /// <summary>
    /// ノード名が最大長を超えている
    /// </summary>
    public sealed class NodeNameTooLongException : Exception
    {
        /// <summary>
        /// ノード名が最大長を超えている
        /// </summary>
        public NodeNameTooLongException() : base($"ノード名は{NodeName.MAX_LENGTH}文字以内で入力してください。")
        {
        }
    }
}
