using System;

namespace AsciiTreeMaker
{
    /*
     * TextFormatTreeBranchLength クラス
     * テキスト変換後の樹形図の枝の長さ
     */
    public class TextFormatTreeBranchLength
    {
        private static readonly int MIN_LENGTH = 0;
        private static readonly int MAX_LENGTH = 5;
        private static readonly int DEFAULT_LENGTH = 2;
        public int Value { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="branchLength">枝の長さ</param>
        public TextFormatTreeBranchLength(in int branchLength)
        {
            if (branchLength < MIN_LENGTH || branchLength > MAX_LENGTH)
            {
                throw new ArgumentOutOfRangeException("TextFormatTreeBranchLength");
            }

            Value = branchLength;
        }

        /// <summary>
        /// 既定値の枝の長さを返す
        /// </summary>
        /// <returns></returns>
        public static TextFormatTreeBranchLength DefaultTextFormatTreeBranchLength()
        {
            return new TextFormatTreeBranchLength(DEFAULT_LENGTH);
        }

        /// <summary>
        /// オブジェクトのコピーを渡す
        /// </summary>
        /// <param name="branchLength"></param>
        /// <returns></returns>
        public static TextFormatTreeBranchLength Copy(in TextFormatTreeBranchLength branchLength)
        {
            return new TextFormatTreeBranchLength(branchLength.Value);
        }

        /// <summary>
        /// 枝の長さを更新する
        /// </summary>
        /// <param name="branchLength"></param>
        public void Update(in TextFormatTreeBranchLength branchLength)
        {
            Value = branchLength.Value;
        }
    }
}
