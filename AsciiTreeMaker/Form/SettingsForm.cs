using System;
using System.Windows.Forms;

namespace AsciiTreeMaker
{
    /*
     * SettingsForm クラス
     * 設定画面を管理する
     */
    public partial class SettingsForm : Form
    {
        private readonly TextFormatTreeBranchLength branchLength;
        public TextFormatTreeBranchLength UpdatedBranchLength { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="branchLength_prev"></param>
        public SettingsForm(in TextFormatTreeBranchLength branchLength_prev)
        {
            InitializeComponent();

            branchLength = branchLength_prev;
            UpdatedBranchLength = TextFormatTreeBranchLength.Copy(branchLength);

            LoadPresentSettings(branchLength);
        }

        /// <summary>
        /// コンボボックスの選択されているアイテムを更新する
        /// </summary>
        /// <param name="branchLength_present"></param>
        private void LoadPresentSettings(in TextFormatTreeBranchLength branchLength_present)
        {
            // 「選択肢のインデックス = 選択肢の値」だが、将来的にずれた時に困るので、安全のため文字列を検索する
            branchLengthComboBox.SelectedIndex = branchLengthComboBox.FindStringExact(branchLength_present.Value.ToString());
        }

        /// <summary>
        /// 枝の長さに関するコンボボックスが変化したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BranchLengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItemText = branchLengthComboBox.SelectedItem.ToString();

            bool success = int.TryParse(selectedItemText, out int selectedBranchLength);
            if (!success) 
            {
                throw new Exception("branchLengthComboBox_SelectedIndexChanged");
            }

            UpdatedBranchLength.Update(new TextFormatTreeBranchLength(selectedBranchLength));
        }

        /// <summary>
        /// "OK"を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// "キャンセル"を押下した際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.Cancel;
            this.Close();
        }
    }
}
