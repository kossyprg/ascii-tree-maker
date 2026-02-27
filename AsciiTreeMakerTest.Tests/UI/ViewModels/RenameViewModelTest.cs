using AsciiTreeMaker.UI.Services;
using AsciiTreeMaker.UI.ViewModels;
using Moq;
using Prism.Services.Dialogs;

namespace AsciiTreeMakerTest.Tests.UI.ViewModels
{
    [TestClass]
    public class RenameViewModelTest
    {
        [TestMethod]
        public void ノード名編集画面_ケース1_正常なノード名()
        {
            var messageService = new Mock<IMessageService>();
            var vm = new RenameViewModel(messageService.Object)
            {
                EditText = "有効なノード名"
            };

            // ウィンドウを閉じた時の結果を受け取るために用意
            IDialogResult? closedResult = null;
            vm.RequestClose += r => closedResult = r;

            // 実行
            vm.OKButtonExecute();

            // テスト
            Assert.IsNotNull(closedResult);
            Assert.AreEqual(ButtonResult.OK, closedResult.Result);
        }

        [TestMethod]
        public void ノード名編集画面_ケース2_空欄あるいは空白文字()
        {
            var messageService = new Mock<IMessageService>();

            var vm = new RenameViewModel(messageService.Object)
            {
                EditText = "   "
            };

            // ウィンドウを閉じた時の結果を受け取るために用意
            IDialogResult? closedResult = null;
            vm.RequestClose += r => closedResult = r;

            // 実行
            vm.OKButtonExecute();

            // バリデーションエラー時はダイアログを閉じない
            Assert.IsNull(closedResult);

            // エラーメッセージ
            messageService.Verify(
                x => x.Error("ノード名を入力してください。", null),
                Times.Once);

            // 他のメソッドは呼び出されていない
            messageService.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void ノード名編集画面_ケース3_ノード名の最大長を超過している()
        {
            var messageService = new Mock<IMessageService>();

            var vm = new RenameViewModel(messageService.Object)
            {
                EditText = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901"
            };

            // ウィンドウを閉じた時の結果を受け取るために用意
            IDialogResult? closedResult = null;
            vm.RequestClose += r => closedResult = r;

            // 実行
            vm.OKButtonExecute();

            // バリデーションエラー時はダイアログを閉じない
            Assert.IsNull(closedResult);

            // エラーメッセージ
            messageService.Verify(
                x => x.Error("ノード名は100文字以内で入力してください。", null),
                Times.Once);

            // 他のメソッドは呼び出されていない
            messageService.VerifyNoOtherCalls();
        }
    }
}
