using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Infrastructure.Json
{
    /// <summary>
    /// ITreeNodeとJSON保存用DTOを相互変換する内部マッパー
    /// </summary>
    internal static class JsonTreeMapper
    {
        /// <summary>
        /// ITreeNode -> Json保存用DTO
        /// </summary>
        /// <param name="node">ITreeNode</param>
        /// <returns>Json保存用DTO</returns>
        public static JsonTreeNodeDto ToDto(ITreeNode node)
        {
            return new JsonTreeNodeDto
            {
                NodeName = node.Name.Value,
                Children = node.Children?.Select(ToDto).ToList() ?? new List<JsonTreeNodeDto>()
            };
        }

        /// <summary>
        /// Json保存用DTO -> ITreeNode
        /// </summary>
        /// <param name="dto">Json保存用DTO</param>
        /// <returns>ITreeNode</returns>
        public static ITreeNode FromDto(JsonTreeNodeDto dto)
        {
            IReadOnlyList<ITreeNode> children =
                dto.Children == null
                    ? Array.Empty<ITreeNode>()
                    : dto.Children.Select(FromDto).ToList().AsReadOnly();

            return new JsonTreeNodeReadOnly(new NodeName(dto.NodeName), children);
        }
    }
}
