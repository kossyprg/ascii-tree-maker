using System.Linq;

namespace AsciiTreeMaker
{
    /*
     * TextFormatTreeDesigner クラス
     * テキストベースの樹形図のデザインを変える
     */
    internal class TextFormatTreeDesigner
    {
        public string LastNodeBranch         { get; private set; } // "└── "
        public string IntermediateNodeBranch { get; private set; } // "├── "
        public string VerticalLine           { get; private set; } // "│   "
        public string Blank                  { get; private set; } // "    "

        private readonly TextFormatTreeBranchLength textFormatTreeBranchLength;
        private static string branchString = "─";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="textFormatTreeBranchLength">枝の長さ</param>
        public TextFormatTreeDesigner(in TextFormatTreeBranchLength textFormatTreeBranchLength)
        {
            this.textFormatTreeBranchLength = textFormatTreeBranchLength;
            SetupTextFormatTreeDesign();
        }

        /// <summary>
        /// 枝の長さを変える
        /// </summary>
        /// <param name="branchLength">枝の長さ</param>
        public void UpdateBranchLength(in TextFormatTreeBranchLength branchLength)
        {
            textFormatTreeBranchLength.Update(branchLength);
            SetupTextFormatTreeDesign();
        }

        /// <summary>
        /// テキストベースの樹形図で用いる枝のデザインを決める
        /// </summary>
        private void SetupTextFormatTreeDesign()
        {
            LastNodeBranch = "└";
            LastNodeBranch += string.Concat(Enumerable.Repeat(branchString, textFormatTreeBranchLength.Value));
            LastNodeBranch += " ";

            IntermediateNodeBranch = "├";
            IntermediateNodeBranch += string.Concat(Enumerable.Repeat(branchString, textFormatTreeBranchLength.Value));
            IntermediateNodeBranch += " ";

            VerticalLine = "│";
            VerticalLine += string.Concat(Enumerable.Repeat(" ", textFormatTreeBranchLength.Value + 1));

            Blank = new string(' ', textFormatTreeBranchLength.Value + 2);
        }
    }
}
