using System.Windows;

namespace AsciiTreeMaker.UI.Services
{
    public interface IMessageService
    {
        /// <summary>
        /// 情報ダイアログ
        /// </summary>
        /// <param name="message">情報</param>
        void Information(string message);

        /// <summary>
        /// 質問ダイアログ
        /// </summary>
        /// <param name="message">質問内容</param>
        /// <param name="caption">キャプション（タイトルに表示される）</param>
        /// <returns></returns>
        MessageBoxResult Question(string message, string caption);

        /// <summary>
        /// エラーダイアログ
        /// </summary>
        /// <param name="message">エラー内容</param>
        /// <param name="detail">(任意) エラー詳細</param>
        void Error(string message, string? detail = null);
    }
}
