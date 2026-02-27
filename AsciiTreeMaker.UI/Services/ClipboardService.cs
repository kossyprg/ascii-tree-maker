using System.Windows;

namespace AsciiTreeMaker.UI.Services
{
    public sealed class ClipboardService : IClipboardService
    {
        /// <summary>
        /// クリップボードにコピーする
        /// </summary>
        /// <param name="text">コピーするテキスト</param>
        public void SetText(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
