using System.Windows;

namespace AsciiTreeMaker.UI.Services
{
    public sealed class MessageService : IMessageService
    {
        /// <summary>
        /// 情報ダイアログ
        /// </summary>
        /// <param name="message">情報</param>
        public void Information(string message)
        {
            MessageBox.Show(
                message,
                "情報",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// 質問ダイアログ
        /// </summary>
        /// <param name="message">質問内容</param>
        /// <param name="caption">キャプション（タイトルに表示される）</param>
        /// <returns></returns>
        public MessageBoxResult Question(string message, string caption)
        {
            return MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);
        }

        /// <summary>
        /// エラーダイアログ
        /// </summary>
        /// <param name="message">エラー内容</param>
        /// <param name="detail">(任意) エラー詳細</param>
        public void Error(string message, string? detail = null)
        {
            if (detail != null)
            {
                message += "\n\n[詳細]\n" + detail;
            }

            MessageBox.Show(
                message,
                "エラー",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
