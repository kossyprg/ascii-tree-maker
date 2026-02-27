using AsciiTreeMaker.Domain.Exceptions;
using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;
using AsciiTreeMaker.Infrastructure.Helpers;
using System.Text;
using System.Text.Json;

namespace AsciiTreeMaker.Infrastructure.Json
{
    internal sealed class TreeFileIOJson : ITreeFileIO
    {
        private const long MAX_FILESIZE_BYTES = 20L * 1024 * 1024; // 20MB

        /// <summary>
        /// 木構造データを読み込む
        /// </summary>
        /// <param name="path">外部ソースの読込元パス</param>
        /// <returns>木構造データ</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public IReadOnlyList<ITreeNode> LoadFrom(string path)
        {
            // ファイルが無ければ「空」
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("対象ファイルが存在しません。");
            }

            if (!string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnsupportedFileFormatException("サポートされていないファイル形式です");
            }

            // ファイルサイズ
            long sizeBytes = new FileInfo(path).Length;
            if (sizeBytes > MAX_FILESIZE_BYTES)
            {
                throw new FileSizeLimitExceededException("ファイルサイズが上限（20MB）を超えているため読み込めません。");
            }

            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw new TreeRestoreException("木構造データの復元に失敗しました。", ex.Message);
            }

            JsonTreeFileDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<JsonTreeFileDto>(json);
            }
            catch (JsonException ex)
            {
                throw new TreeRestoreException("木構造データの復元に失敗しました。", ex.Message, innerException: ex);
            }

            if (dto == null || dto.Roots == null || dto.Roots.Count == 0)
            {
                throw new TreeRestoreException("ファイルの内容が不正です", "データが欠損しているかノード数が0です。");
            }

            // 最大階層のバリデーションを入れる
            try
            {
                foreach (var root in dto.Roots)
                {
                    Search(root, 1);
                }
            }
            catch
            {
                // 再スロー
                throw;
            }

            try
            {
                return dto.Roots
                    .Select(JsonTreeMapper.FromDto)
                    .ToList()
                    .AsReadOnly();

            }
            catch (NodeNameIsEmptyException)
            {
                throw new TreeRestoreException(
                    "木構造データの復元に失敗しました。",
                    "ノード名が空欄あるいは空白文字のみのノードが含まれています。");
            }
            catch (NodeNameTooLongException)
            {
                throw new TreeRestoreException(
                    "木構造データの復元に失敗しました。",
                    "ノード名の最大長を超えるノードを含んでいます。");
            }
            catch (Exception ex)
            {
                // 原因不明の例外
                throw new TreeRestoreException(
                    "木構造データの復元に失敗しました。",
                    string.Empty,
                    innerException: ex);
            }
        }

        /// <summary>
        /// 木構造データを保存する
        /// </summary>
        /// <param name="path">外部ソースの保存先パス</param>
        /// <param name="roots">木構造データ</param>
        public void SaveTo(string path, IReadOnlyList<ITreeNode> roots)
        {
            if (roots == null) throw new ArgumentNullException(nameof(roots));

            var dto = new JsonTreeFileDto
            {
                Roots = roots.Select(JsonTreeMapper.ToDto).ToList()
            };

            string json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });

            long sizeBytes = Encoding.UTF8.GetByteCount(json);

            // ファイルサイズが上限を超えている
            if (sizeBytes > MAX_FILESIZE_BYTES)
            {
                throw new FileSizeLimitExceededException("ファイルサイズが上限（20MB）を超えているため保存できません。");
            }

            try
            {
                FileHelper.WriteAtomically(path, json);
            }
            catch (Exception ex)
            {
                throw new FileWriteException("ファイルの保存に失敗しました。", ex);
            }
        }

        private void Search(JsonTreeNodeDto node, int depth)
        {
            if (depth > NodeDepth.MAX)
            {
                throw new TreeRestoreException(
                    "木構造データの復元に失敗しました。",
                    "最大階層を超えるノードを含んでいます。");
            }

            foreach (var child in node.Children)
            {
                Search(child, depth + 1);
            }
        }
    }
}
