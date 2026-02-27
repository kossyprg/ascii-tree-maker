using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Infrastructure.Json
{
    /// <summary>
    /// 木構造データの1ノードを表す DTO(Data Transfer Object)
    /// </summary>
    internal sealed class JsonTreeNodeDto
    {
        /// <summary>
        /// ノード名
        /// </summary>
        public string NodeName { get; set; } = string.Empty;

        /// <summary>
        /// 子ノード
        /// </summary>
        public List<JsonTreeNodeDto> Children { get; set; } = new();
    }
}
