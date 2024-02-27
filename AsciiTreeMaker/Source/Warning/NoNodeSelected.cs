using System.Windows.Forms;

namespace AsciiTreeMaker.Source
{
    internal class NoNodeSelected : IWarning
    {
        public DialogResult ShowWarningMessageBox()
        {
            string message = "ノードが選択されていません";

            DialogResult result = MessageBox.Show(message,
                                        AppBaseInfo.APP_NAME,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1);
            return result;
        }
    }
}
