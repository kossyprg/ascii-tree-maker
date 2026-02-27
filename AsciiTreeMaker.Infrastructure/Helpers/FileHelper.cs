using System.Text;

namespace AsciiTreeMaker.Infrastructure.Helpers
{
    /// <summary>
    /// ファイル操作関係のヘルパー
    /// </summary>
    internal static class FileHelper
    {
        /// <summary>
        /// ユーザーの LocalApplicationData 配下にある
        /// AsciiTreeMaker 専用フォルダ内のファイルパスを取得する。
        /// </summary>
        /// <param name="fileName">ファイル名(拡張子を含む)</param>
        /// <returns>フルパス</returns>
        public static string GetLocalAppFilePath(string fileName)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDir = Path.Combine(appData, "AsciiTreeMaker");
            return Path.Combine(appDir, fileName);
        }

        /// <summary>
        /// 指定されたファイルパスの親ディレクトリが存在することを保証する
        /// </summary>
        /// <param name="filePath">ファイルのフルパス</param>
        public static void EnsureDirectoryExists(string filePath)
        {
            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                Directory.CreateDirectory(dir); // 無ければ作成、存在すれば何もしない
            }
        }

        /// <summary>
        /// ファイルを可能な限り安全に書き込む。
        /// 一時ファイルに完全に書き込んでから差し替えることで、クラッシュ等に強くする。
        /// </summary>
        /// <param name="path">書き込み先ファイルパス</param>
        /// <param name="content">書き込む内容</param>
        public static void WriteAtomically(string path, string content)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("path is null or empty.", nameof(path));
            }

            EnsureDirectoryExists(path);

            // 同一ディレクトリに temp を作る（同一ボリューム内の置き換え/移動を保証）
            string? directory = Path.GetDirectoryName(path) ?? "";
            string fileName = Path.GetFileName(path);

            // 競合しにくい一時ファイル名を用意
            string temp = Path.Combine(directory, $".{fileName}.{Guid.NewGuid():N}.tmp");

            try
            {
                // 一時ファイルに完全に書き込み
                using (var fs = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (var sw = new StreamWriter(fs, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)))
                {
                    sw.Write(content);
                    sw.Flush();
                    fs.Flush(flushToDisk: true);
                }

                if (File.Exists(path))
                {
                    File.Replace(temp, path, destinationBackupFileName: null);
                }
                else
                {
                    File.Move(temp, path);
                }
            }
            catch
            {
                // 失敗しても元ファイルは極力触らない
                try
                {
                    if (File.Exists(temp))
                    {
                        File.Delete(temp);
                    }
                }
                catch
                {
                    // 後片付け失敗は握りつぶし
                    // 元ファイルを破損させないことが優先
                }

                throw;
            }
        }
    }
}
