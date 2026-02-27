 namespace AsciiTreeMaker.Infrastructure.Json
{
    /// <summary>
    /// 木構造をファイルに保存するための DTO(Data Transfer Object)
    /// </summary>
    internal sealed class JsonTreeFileDto
    {
        public List<JsonTreeNodeDto> Roots { get; set; } = new();
    }
}
