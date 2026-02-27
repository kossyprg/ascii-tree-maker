using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;
using AsciiTreeMaker.Infrastructure.Helpers;
using System.Text.Json;

namespace AsciiTreeMaker.Infrastructure.Json
{
    /// <summary>
    /// JSON 形式で木構造データを永続化するクラス
    /// </summary>
    internal sealed class TreeRepositoryJson : ITreeRepository
    {
        private readonly string _jsonPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TreeRepositoryJson()
        {
            _jsonPath = FileHelper.GetLocalAppFilePath("tree.json");
            FileHelper.EnsureDirectoryExists(_jsonPath);
        }

        /// <summary>
        /// 木構造のルートノード群を読み込む
        /// </summary>
        /// <returns>木構造のルートノード群</returns>
        public IReadOnlyList<ITreeNode> LoadTree()
        {
            // ファイルが無ければ「空」
            if (!File.Exists(_jsonPath))
            {
                return CreatePlaceholderTree();
            }

            try
            {
                var json = File.ReadAllText(_jsonPath);
                var dto = JsonSerializer.Deserialize<JsonTreeFileDto>(json);

                if (dto == null || dto.Roots == null || dto.Roots.Count == 0)
                {
                    return CreatePlaceholderTree();
                }

                return dto.Roots
                .Select(JsonTreeMapper.FromDto)
                .ToList()
                .AsReadOnly();
            }
            catch
            {
                return CreatePlaceholderTree();
            }
        }

        /// <summary>
        /// 木構造データを保存する
        /// </summary>
        /// <param name="roots">木構造のルートノード群</param>
        public void SaveTree(IReadOnlyList<ITreeNode> roots)
        {
            if (roots == null) throw new ArgumentNullException(nameof(roots));

            FileHelper.EnsureDirectoryExists(_jsonPath);

            var dto = new JsonTreeFileDto
            {
                Roots = roots.Select(JsonTreeMapper.ToDto).ToList()
            };

            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });

            // 安全に保存
            var temp = _jsonPath + ".tmp";
            File.WriteAllText(temp, json);

            if (File.Exists(_jsonPath))
            {
                try
                {
                    File.Replace(temp, _jsonPath, destinationBackupFileName: null);
                    return;
                }
                catch
                {
                    File.Delete(_jsonPath);
                }
            }

            File.Move(temp, _jsonPath);
        }

        private static IReadOnlyList<ITreeNode> CreatePlaceholderTree()
        {
            return new List<ITreeNode>
                {
                    new JsonTreeNodeReadOnly(NodeName.GetInitialNodeName(), Array.Empty<ITreeNode>())
                }.AsReadOnly();
        }
    }
}
