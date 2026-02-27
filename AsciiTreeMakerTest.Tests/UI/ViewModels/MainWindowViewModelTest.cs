using AsciiTreeMaker.Domain.Entities;
using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;
using AsciiTreeMaker.Infrastructure;
using AsciiTreeMaker.UI.Models;
using AsciiTreeMaker.UI.Services;
using AsciiTreeMaker.UI.ViewModels;
using AsciiTreeMaker.UI.Views;
using AsciiTreeMakerTest.Tests.TestHelpers;
using Moq;
using Prism.Services.Dialogs;
using System.Xml.Linq;

namespace AsciiTreeMakerTest.Tests.UI.ViewModels
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        [TestMethod]
        public void 木構造データ初期状態()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object, 
                messageService.Object, 
                treeMock, 
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.AreEqual("0", vm.Roots[0].Name.Value);
            Assert.AreEqual("0-0", vm.Roots[0].Children[0].Name.Value);
            Assert.AreEqual("0-1", vm.Roots[0].Children[1].Name.Value);
            Assert.AreEqual("0-1-0", vm.Roots[0].Children[1].Children[0].Name.Value);
            Assert.AreEqual("0-2", vm.Roots[0].Children[2].Name.Value);
            Assert.AreEqual("0-2-0", vm.Roots[0].Children[2].Children[0].Name.Value);
            Assert.AreEqual("0-2-0-0", vm.Roots[0].Children[2].Children[0].Children[0].Name.Value);
            Assert.AreEqual("0-2-0-1", vm.Roots[0].Children[2].Children[0].Children[1].Name.Value);
            Assert.AreEqual("0-2-1", vm.Roots[0].Children[2].Children[1].Name.Value);
            Assert.AreEqual("0-3", vm.Roots[0].Children[3].Name.Value);

            // 選択状態の確認
            Assert.IsTrue(vm.Roots[0].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[0].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[1].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[1].Children[0].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[2].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].Children[0].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].Children[1].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[1].IsSelected);
            Assert.IsFalse(vm.Roots[0].Children[3].IsSelected);

            // すべて折りたたまれた状態
            Assert.IsFalse(vm.Roots[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[1].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[1].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].Children[1].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[1].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[3].IsExpanded);

            // ノード数
            Assert.AreEqual(10, vm.Roots[0].TotalNodeCount);
            Assert.AreEqual(1, vm.Roots[0].Children[0].TotalNodeCount);
            Assert.AreEqual(2, vm.Roots[0].Children[1].TotalNodeCount);
            Assert.AreEqual(5, vm.Roots[0].Children[2].TotalNodeCount);
            Assert.AreEqual(1, vm.Roots[0].Children[3].TotalNodeCount);
            Assert.AreEqual(1, vm.Roots[0].Children[2].Children[0].Children[0].TotalNodeCount);

            // 階層
            Assert.AreEqual(1, vm.Roots[0].Depth);
            Assert.AreEqual(2, vm.Roots[0].Children[0].Depth);
            Assert.AreEqual(2, vm.Roots[0].Children[1].Depth);
            Assert.AreEqual(2, vm.Roots[0].Children[2].Depth);
            Assert.AreEqual(2, vm.Roots[0].Children[3].Depth);
            Assert.AreEqual(3, vm.Roots[0].Children[1].Children[0].Depth);
            Assert.AreEqual(3, vm.Roots[0].Children[2].Children[0].Depth);
            Assert.AreEqual(3, vm.Roots[0].Children[2].Children[1].Depth);
            Assert.AreEqual(4, vm.Roots[0].Children[2].Children[0].Children[0].Depth);
            Assert.AreEqual(4, vm.Roots[0].Children[2].Children[0].Children[1].Depth);
        }

        [TestMethod]
        public void 木構造データ編集機能_子ノードを追加_ケース1_正常な実行()
        {
            var dialogService = new Mock<IDialogService>();

            // コールバックに擬似結果を流し込む
            // ノード名「子ノード」を追加する想定
            dialogService.Setup(x => x.ShowDialog(
                    nameof(RenameView),
                    It.IsAny<IDialogParameters>(),
                    It.IsAny<Action<IDialogResult>>()))
                .Callback<string, IDialogParameters, Action<IDialogResult>>(
                    (name, parameters, callback) =>
                    {
                        var dialogParams = new DialogParameters
                        {
                        { nameof(RenameViewModel.EditText), new NodeName("子ノード") }
                        };

                        var result = new DialogResult(ButtonResult.OK, dialogParams);

                        // ここで ViewModel の RenameViewClose が呼ばれる
                        callback(result);
                    });

            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object, 
                messageService.Object, 
                treeMock, 
                userPreferencesRepo.Object,
                clipboardService.Object);

            vm.SelectedNode = vm.Roots[0].Children[0];
            var parent = vm.SelectedNode;

            // コマンドが実行可能であること（SelectedNode != null）
            Assert.IsTrue(vm.AddChildCommand.CanExecute());

            // 実行
            vm.AddChildCommand.Execute();

            // テスト
            var added = parent.Children.Last();            
            Assert.AreEqual("子ノード", added.Name.Value);
            Assert.AreEqual(2, parent.Depth);
            Assert.AreEqual(3, added.Depth);

            // 子ノードの兄弟ノードが展開されていること
            Assert.IsTrue(parent.IsExpanded);

            // 追加した子ノードが選択されていること
            Assert.IsTrue(added.IsSelected);

            // Document.IsDirty など、他の副作用も確認する
            Assert.IsTrue(vm.Document.IsDirty);

            // ダイアログが 1 回呼ばれたことを確認
            dialogService.Verify(x => x.ShowDialog(
                nameof(RenameView),
                It.IsAny<IDialogParameters>(),
                It.IsAny<Action<IDialogResult>>()),
                Times.Once);
        }

        [TestMethod]
        public void 木構造データ編集機能_子ノードを追加_ケース2_最大階層を超える場合は実行不可()
        {
            var dialogService = new Mock<IDialogService>();

            // コールバックに擬似結果を流し込む
            // ノード名「子ノード」を追加する想定
            dialogService.Setup(x => x.ShowDialog(
                    nameof(RenameView),
                    It.IsAny<IDialogParameters>(),
                    It.IsAny<Action<IDialogResult>>()))
                .Callback<string, IDialogParameters, Action<IDialogResult>>(
                    (name, parameters, callback) =>
                    {
                        var dialogParams = new DialogParameters
                        {
                        { nameof(RenameViewModel.EditText), new NodeName("子ノード") }
                        };

                        var result = new DialogResult(ButtonResult.OK, dialogParams);

                        // ここで ViewModel の RenameViewClose が呼ばれる
                        callback(result);
                    });

            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.MaxDepth); // 最大階層を持つ木構造データを使う
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 10階層目まで進める
            var children = vm.Roots[0].Children;
            for (int i = 0; i < 8; i++)
            {
                children = children[0].Children;
            }
            TreeViewNode node = children[0];
            vm.SelectedNode = node;
            Assert.AreEqual(10, node.Depth);

            // コマンドが実行不可能であること
            Assert.IsFalse(vm.AddChildCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_子ノードを追加_ケース3_選択ノードがないときは実行不可()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            vm.SelectedNode = null;
            Assert.IsFalse(vm.AddChildCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_兄弟ノードを追加_ケース1_正常な実行()
        {
            var dialogService = new Mock<IDialogService>();

            // コールバックに擬似結果を流し込む
            // ノード名「兄弟ノード」を追加する想定
            dialogService.Setup(x => x.ShowDialog(
                    nameof(RenameView),
                    It.IsAny<IDialogParameters>(),
                    It.IsAny<Action<IDialogResult>>()))
                .Callback<string, IDialogParameters, Action<IDialogResult>>(
                    (name, parameters, callback) =>
                    {
                        var dialogParams = new DialogParameters
                        {
                        { nameof(RenameViewModel.EditText), new NodeName("兄弟ノード") }
                        };

                        var result = new DialogResult(ButtonResult.OK, dialogParams);

                        // ここで ViewModel の RenameViewClose が呼ばれる
                        callback(result);
                    });

            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            var node = vm.Roots[0].Children[0];
            vm.SelectedNode = node;
            Assert.AreEqual("0-0", node.Name.Value);
            Assert.AreEqual(2, node.Depth);

            // コマンドが実行可能であること（SelectedNode != null）
            Assert.IsTrue(vm.AddBrotherCommand.CanExecute());

            // 実行
            vm.AddBrotherCommand.Execute();

            // テスト
            TreeViewNode brother = node.Parent!.Children[1];
            Assert.AreEqual("兄弟ノード", brother.Name.Value);
            Assert.AreEqual(2, brother.Depth);
            Assert.IsFalse(node.IsSelected);
            Assert.IsTrue(brother.IsSelected);
            Assert.IsTrue(vm.Document.IsDirty);

            // ダイアログが 1 回呼ばれたことを確認
            dialogService.Verify(x => x.ShowDialog(
                nameof(RenameView),
                It.IsAny<IDialogParameters>(),
                It.IsAny<Action<IDialogResult>>()),
                Times.Once);
        }

        [TestMethod]
        public void 木構造データ編集機能_兄弟ノードを追加_ケース2_選択ノードがないときは実行不可()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            vm.SelectedNode = null;
            Assert.IsFalse(vm.AddBrotherCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードを移動_ケース1_下方向()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 移動前
            Assert.AreEqual("0-0", vm.Roots[0].Children[0].Name.Value);
            Assert.AreEqual("0-1", vm.Roots[0].Children[1].Name.Value);
            Assert.IsFalse(vm.Document.IsDirty);

            var node = vm.Roots[0].Children[0];
            vm.SelectedNode = node;

            // 下に移動
            Assert.IsTrue(vm.MoveDownCommand.CanExecute());
            vm.MoveDownCommand.Execute();

            // 移動後
            Assert.AreEqual("0-1", vm.Roots[0].Children[0].Name.Value);
            Assert.AreEqual("0-0", vm.Roots[0].Children[1].Name.Value);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードを移動_ケース2_上方向()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 移動前
            Assert.AreEqual("0-1", vm.Roots[0].Children[1].Name.Value);
            Assert.AreEqual("0-2", vm.Roots[0].Children[2].Name.Value);
            Assert.IsFalse(vm.Document.IsDirty);

            var node = vm.Roots[0].Children[2];
            vm.SelectedNode = node;

            // 下に移動
            Assert.IsTrue(vm.MoveUpCommand.CanExecute());
            vm.MoveUpCommand.Execute();

            // 移動後
            Assert.AreEqual("0-2", vm.Roots[0].Children[1].Name.Value);
            Assert.AreEqual("0-1", vm.Roots[0].Children[2].Name.Value);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードを移動_ケース3_下方向_端の場合は実行不可()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 移動前
            var node = vm.Roots[0].Children[3];
            Assert.AreEqual("0-3", node.Name.Value);
            vm.SelectedNode = node;

            // 下に移動不可
            Assert.IsFalse(vm.MoveDownCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードを移動_ケース4_上方向_端の場合は実行不可()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 移動前
            var node = vm.Roots[0].Children[0];
            Assert.AreEqual("0-0", node.Name.Value);
            vm.SelectedNode = node;

            // 上に移動不可
            Assert.IsFalse(vm.MoveUpCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_ノード名の編集_ケース1_正常な実行()
        {
            var dialogService = new Mock<IDialogService>();

            // コールバックに擬似結果を流し込む
            // ノード名を「名称変更後ノード」に変更する想定
            dialogService.Setup(x => x.ShowDialog(
                    nameof(RenameView),
                    It.IsAny<IDialogParameters>(),
                    It.IsAny<Action<IDialogResult>>()))
                .Callback<string, IDialogParameters, Action<IDialogResult>>(
                    (name, parameters, callback) =>
                    {
                        var dialogParams = new DialogParameters
                        {
                        { nameof(RenameViewModel.EditText), new NodeName("名称変更後ノード") }
                        };

                        var result = new DialogResult(ButtonResult.OK, dialogParams);

                        // ここで ViewModel の RenameViewClose が呼ばれる
                        callback(result);
                    });

            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            var node = vm.Roots[0].Children[1];
            vm.SelectedNode = node;

            node.IsSelected = true;
            Assert.AreEqual(node, vm.SelectedNode);
            Assert.AreEqual("0-1", node.Name.Value);
            Assert.IsTrue(node.IsSelected);

            // 実行
            vm.RenameCommand.Execute();

            Assert.AreEqual("名称変更後ノード", node.Name.Value);
            Assert.IsTrue(node.IsSelected);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードの削除_ケース1_削除するノードの下に兄弟ノードがある()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 実行前
            Assert.AreEqual("0-1", vm.Roots[0].Children[1].Name.Value);
            Assert.AreEqual("0-2", vm.Roots[0].Children[2].Name.Value);
            Assert.IsFalse(vm.Document.IsDirty);

            var node = vm.Roots[0].Children[1];
            vm.SelectedNode = node;

            // 実行
            Assert.IsTrue(vm.RemoveCommand.CanExecute());
            vm.RemoveCommand.Execute();

            // 実行後
            Assert.AreEqual("0-2", vm.Roots[0].Children[1].Name.Value);
            Assert.AreEqual("0-3", vm.Roots[0].Children[2].Name.Value);
            Assert.IsTrue(vm.Roots[0].Children[1].IsSelected);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードの削除_ケース2_削除するノードの下に兄弟ノードがない()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 実行前
            Assert.AreEqual("0-2", vm.Roots[0].Children[2].Name.Value);
            Assert.AreEqual("0-3", vm.Roots[0].Children[3].Name.Value);
            Assert.IsFalse(vm.Document.IsDirty);

            var node = vm.Roots[0].Children[3];
            vm.SelectedNode = node;

            // 実行
            Assert.IsTrue(vm.RemoveCommand.CanExecute());
            vm.RemoveCommand.Execute();

            // 実行後
            Assert.AreEqual("0-2", vm.Roots[0].Children[2].Name.Value);
            Assert.IsTrue(vm.Roots[0].Children[2].IsSelected);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードの削除_ケース3_兄弟ノードがない()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 実行前
            Assert.AreEqual("0-1", vm.Roots[0].Children[1].Name.Value);
            Assert.AreEqual("0-1-0", vm.Roots[0].Children[1].Children[0].Name.Value);
            Assert.IsFalse(vm.Document.IsDirty);

            var node = vm.Roots[0].Children[1].Children[0];
            vm.SelectedNode = node;

            // 実行
            Assert.IsTrue(vm.RemoveCommand.CanExecute());
            vm.RemoveCommand.Execute();

            // 実行後
            Assert.IsTrue(vm.Roots[0].Children[1].IsSelected);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードの削除_ケース4_ルートノードを削除()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            // 実行前
            Assert.AreEqual("0", vm.Roots[0].Name.Value);
            Assert.IsFalse(vm.Document.IsDirty);

            var node = vm.Roots[0];
            vm.SelectedNode = node;

            // 実行
            Assert.IsTrue(vm.RemoveCommand.CanExecute());
            vm.RemoveCommand.Execute();

            // 実行後
            Assert.AreEqual("テキストを入力", vm.Roots[0].Name.Value);
            Assert.IsTrue(vm.Roots[0].IsSelected);
            Assert.IsTrue(vm.Document.IsDirty);
        }

        [TestMethod]
        public void 木構造データ編集機能_ノードの削除_ケース5_選択ノードがないときは削除不可()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            vm.SelectedNode = null;
            Assert.IsFalse(vm.RemoveCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_すべて展開_ケース1_正常系()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.IsTrue(vm.ExpandAllCommand.CanExecute());
            vm.ExpandAllCommand.Execute();

            Assert.IsTrue(vm.Roots[0].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[0].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[1].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[1].Children[0].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[2].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[2].Children[0].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[2].Children[0].Children[0].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[2].Children[0].Children[1].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[2].Children[1].IsExpanded);
            Assert.IsTrue(vm.Roots[0].Children[3].IsExpanded);
        }

        [TestMethod]
        public void 木構造データ編集機能_すべて展開_ケース2_ノード数が500以上3000未満()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.NodeCountIs511);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.AreEqual(511, vm.Roots[0].TotalNodeCount);
            Assert.AreEqual(511, vm.TotalNodeCount.Value);

            // 実行可能
            Assert.IsTrue(vm.ExpandAllCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_すべて展開_ケース3_ノード数が3000以上()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.NodeCountIs3000);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.AreEqual(3000, vm.Roots[0].TotalNodeCount);
            Assert.AreEqual(3000, vm.TotalNodeCount.Value);

            // 実行不可能
            Assert.IsFalse(vm.ExpandAllCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_すべて折りたたむ_ケース1_正常系()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.IsTrue(vm.CollapseAllCommand.CanExecute());
            vm.CollapseAllCommand.Execute();

            Assert.IsFalse(vm.Roots[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[1].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[1].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].Children[0].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[0].Children[1].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[2].Children[1].IsExpanded);
            Assert.IsFalse(vm.Roots[0].Children[3].IsExpanded);
        }

        [TestMethod]
        public void 木構造データ編集機能_すべて折りたたむ_ケース2_ノード数が500以上3000未満()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.NodeCountIs511);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.AreEqual(511, vm.Roots[0].TotalNodeCount);
            Assert.AreEqual(511, vm.TotalNodeCount.Value);

            // 実行可能
            Assert.IsTrue(vm.CollapseAllCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データ編集機能_すべて折りたたむ_ケース3_ノード数が3000以上()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.NodeCountIs3000);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.AreEqual(3000, vm.Roots[0].TotalNodeCount);
            Assert.AreEqual(3000, vm.TotalNodeCount.Value);

            // 実行不可能
            Assert.IsFalse(vm.CollapseAllCommand.CanExecute());
        }

        [TestMethod]
        public void 木構造データテキスト出力機能()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var treeMock = new TreeMock(TreeMock.TreeMockKind.Typical);
            var userPreferencesRepo = new Mock<IUserPreferencesRepository>();
            userPreferencesRepo.Setup(x => x.Load()).Returns(new UserPreferenceEntity(new BranchLength(2)));
            var clipboardService = new Mock<IClipboardService>();

            string? copied = null;
            clipboardService
                .Setup(x => x.SetText(It.IsAny<string>()))
                .Callback<string>(text => copied = text);

            var vm = new MainWindowViewModel(
                dialogService.Object,
                messageService.Object,
                treeMock,
                userPreferencesRepo.Object,
                clipboardService.Object);

            Assert.IsTrue(vm.CopyToClipBoardCommand.CanExecute());
            vm.CopyToClipBoardCommand.Execute();

            string expected =
@"0
├── 0-0
├── 0-1
│   └── 0-1-0
├── 0-2
│   ├── 0-2-0
│   │   ├── 0-2-0-0
│   │   └── 0-2-0-1
│   └── 0-2-1
└── 0-3
";
            Assert.AreEqual(expected, copied);

            // 情報メッセージを出していること
            messageService.Verify(
                x => x.Information("クリップボードにコピーしました。"),
                Times.Once);
        }
    }
}
