namespace AsciiTreeMaker.UI.Services
{
    public interface IClipboardService
    {
        /// <summary>
        /// クリップボードにコピーする
        /// </summary>
        /// <param name="text">コピーするテキスト</param>
        void SetText(string text);
    }
}
