using System.Windows.Forms;

namespace AsciiTreeMaker.Source
{
    internal class TreeViewIsEmpty : IWarning
    {
        public DialogResult ShowWarningMessageBox()
        {
            string message = "樹形図が空です";

            DialogResult result = MessageBox.Show(message,
                                        AppBaseInfo.APP_NAME,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1);
            return result;
        }
    }
}
