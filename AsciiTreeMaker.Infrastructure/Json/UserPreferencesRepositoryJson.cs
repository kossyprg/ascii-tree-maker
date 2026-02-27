using AsciiTreeMaker.Domain.Entities;
using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;
using AsciiTreeMaker.Infrastructure.Helpers;
using System.Text.Json;

namespace AsciiTreeMaker.Infrastructure.Json
{
    public class UserPreferencesRepositoryJson : IUserPreferencesRepository
    {
        private readonly string _filePath;

        public UserPreferencesRepositoryJson()
        {
            _filePath = FileHelper.GetLocalAppFilePath("userPreferences.json");
            FileHelper.EnsureDirectoryExists(_filePath);
        }

        public void Save(UserPreferenceEntity preferences)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            JsonUserPreferenceEntityDto dto = JsonUserPreferenceEntityMapper.ToDto(preferences);
            string json = JsonSerializer.Serialize(dto, options);
            FileHelper.WriteAtomically(_filePath, json);
        }

        public UserPreferenceEntity Load()
        {
            if (!File.Exists(_filePath))
            {
                return UserPreferenceEntity.GetInitialUserPreferenceEntity();
            }

            var json = File.ReadAllText(_filePath);
            var dto = JsonSerializer.Deserialize<JsonUserPreferenceEntityDto>(json);
            if (dto == null)
            {
                return UserPreferenceEntity.GetInitialUserPreferenceEntity();
            }

            var preferences = JsonUserPreferenceEntityMapper.FromDto(dto);
            return preferences;
        }
    }

    /// <summary>
    /// ユーザ設定用マッパー関数
    /// </summary>
    internal static class JsonUserPreferenceEntityMapper
    {
        /// <summary>
        /// ユーザ設定エンティティ -> DTO
        /// </summary>
        /// <param name="entity">ユーザ設定エンティティ</param>
        /// <returns>DTO</returns>
        public static JsonUserPreferenceEntityDto ToDto(UserPreferenceEntity entity)
        {
            return new JsonUserPreferenceEntityDto
            {
                BranchLength = entity.BranchLength.Value
            };
        }

        /// <summary>
        /// DTO -> ユーザ設定エンティティ
        /// </summary>
        /// <param name="dto">DTO</param>
        /// <returns>ユーザ設定エンティティ</returns>
        public static UserPreferenceEntity FromDto(JsonUserPreferenceEntityDto dto)
        {
            return new UserPreferenceEntity(new BranchLength(dto.BranchLength));
        }
    }

    /// <summary>
    /// Json形式用ユーザ設定DTO
    /// </summary>
    internal class JsonUserPreferenceEntityDto
    {
        /// <summary>
        /// 枝の長さ
        /// </summary>
        public int BranchLength { get; set; }
    }
}
