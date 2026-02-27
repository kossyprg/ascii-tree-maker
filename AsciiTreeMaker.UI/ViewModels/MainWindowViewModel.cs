using AsciiTreeMaker.Domain.Entities;
using AsciiTreeMaker.Domain.Exceptions;
using AsciiTreeMaker.Domain.Logics;
using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;
using AsciiTreeMaker.Infrastructure;
using AsciiTreeMaker.Infrastructure.Json;
using AsciiTreeMaker.UI.BackgroundWorkers;
using AsciiTreeMaker.UI.Models;
using AsciiTreeMaker.UI.Services;
using AsciiTreeMaker.UI.Views;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace AsciiTreeMaker.UI.ViewModels
{
    /// <summary>
    /// メイン画面
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private const string APP_TITLE = "AsciiTreeMaker";
        private readonly IDialogService _dialogService;
        private readonly IMessageService _messageService;
        private readonly ITreeRepository _treeRepository;
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IClipboardService _clipboardService;

        public MainWindowViewModel(IDialogService dialogService) 
            : this(dialogService, 
                  new MessageService(), 
                  Factories.CreateTreeRepository(),
                  new UserPreferencesRepositoryJson(),
                  new ClipboardService())
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel(
            IDialogService dialogService,
            IMessageService messageService,
            ITreeRepository treeRepository,
            IUserPreferencesRepository userPreferencesRepository,
            IClipboardService clipboardService)
        {
            IsDebugMode = false;
#if DEBUG
            IsDebugMode = true;
#endif

            _dialogService = dialogService;
            _messageService = messageService;
            _treeRepository = treeRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _clipboardService = clipboardService;

            // ユーザ設定ファイルの反映
            UserPreferenceEntity userPreferences = _userPreferencesRepository.Load();
            _branchLength = userPreferences.BranchLength;

            var domainRoots = _treeRepository.LoadTree();
            var uiRoots = TreeNodeMapper.ToUi(domainRoots);

            foreach (var r in uiRoots)
            {
                Roots.Add(r);
            }

            // ツールバーのイベントを登録
            AddChildCommand = new DelegateCommand(AddChild, CanAddChild)
                .ObservesProperty(() => SelectedNode);

            AddBrotherCommand = new DelegateCommand(AddBrother, () => SelectedNode != null)
                .ObservesProperty(() => SelectedNode);

            MoveUpCommand = new DelegateCommand(MoveUp, () => CanMove(-1))
                .ObservesProperty(() => SelectedNode);

            MoveDownCommand = new DelegateCommand(MoveDown, () => CanMove(+1))
                .ObservesProperty(() => SelectedNode);

            RenameCommand = new DelegateCommand(Rename, () => SelectedNode != null)
                .ObservesProperty(() => SelectedNode);

            RemoveCommand = new DelegateCommand(Remove, () => SelectedNode != null)
                .ObservesProperty(() => SelectedNode);

            ExpandAllCommand = new DelegateCommand(() => SetExpandedAll(true), CanSetExpandedAll)
                .ObservesProperty(() => TotalNodeCount);

            CollapseAllCommand = new DelegateCommand(() => SetExpandedAll(false), CanSetExpandedAll)
                .ObservesProperty(() => TotalNodeCount);

            CopyToClipBoardCommand = new DelegateCommand(ConvertTreeToText);

            // メニューバーのイベントを登録
            CreateNewCommand = new DelegateCommand(CreateNew);
            OpenCommand = new DelegateCommand(Open);
            SaveCommand = new DelegateCommand(Save);
            SaveAsCommand = new DelegateCommand(SaveAs);
            QuitCommand = new DelegateCommand(Quit);
            
            TreeViewSelectionChanged = new DelegateCommand<object>(TreeViewSelectedItemChangedExecute);

            // Document のプロパティが変化したときにアプリのタイトルを更新するように設定する
            Document.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(TreeDocument.IsDirty)
                 || e.PropertyName == nameof(TreeDocument.FilePath))
                {
                    UpdateAppTitle();
                }
            };

            Roots[0].IsSelected = true;
            SelectedNode = Roots[0];

            SetTotalNodeCount();
        }

        private string _title = APP_TITLE;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// デバッグ実効ならTrue
        /// </summary>
        public bool IsDebugMode { get; }

        private BranchLength _branchLength;
        public int BranchLengthValue
        {
            get { return _branchLength.Value; }
            set { _branchLength = new BranchLength(value); }
        }

        private TreeViewNode? _selectedNode;
        public TreeViewNode? SelectedNode
        {
            get { return _selectedNode; }
            set { SetProperty(ref _selectedNode, value); }
        }

        private NodeName? _renamedNodeName;

        /// <summary>
        /// 総ノード数
        /// </summary>
        private NodeCount _totalNodeCount = new(1);

        /// <summary>
        /// 総ノード数
        /// </summary>
        public NodeCount TotalNodeCount
        {
            get { return _totalNodeCount; }
        }

        /// <summary>
        /// データ永続化用デバウンスタイマー
        /// </summary>
        private readonly DebounceTimer _persistTimer = new(TimeSpan.FromSeconds(1));

        public IReadOnlyList<int> BranchLengthCandidates => BranchLength.Candidates;
        public TreeDocument Document { get; } = new();
        public ObservableCollection<TreeViewNode> Roots { get; } = new();
        public DelegateCommand<object> TreeViewSelectionChanged { get; }
        public DelegateCommand AddChildCommand { get; }
        public DelegateCommand AddBrotherCommand { get; }
        public DelegateCommand MoveUpCommand { get; }
        public DelegateCommand MoveDownCommand { get; }
        public DelegateCommand RenameCommand { get; }
        public DelegateCommand RemoveCommand { get; }
        public DelegateCommand ExpandAllCommand { get; }
        public DelegateCommand CollapseAllCommand { get; }
        public DelegateCommand CopyToClipBoardCommand { get; }
        public DelegateCommand CreateNewCommand { get; }
        public DelegateCommand OpenCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SaveAsCommand { get; }
        public DelegateCommand QuitCommand { get; }

        #region [Methods]
        /// <summary>
        /// 子ノードを追加する
        /// </summary>
        private void AddChild()
        {
            // ボタンを無効化しているはずだが念のため
            if (SelectedNode == null)
            {
                return;
            }

            var p = new DialogParameters
            {
                { nameof(RenameViewModel.EditText), string.Empty }
            };
            _dialogService.ShowDialog(nameof(RenameView), p, RenameViewClose);

            // ノード名が入力されていないなら何もしない
            if (_renamedNodeName is null)
            {
                return;
            }

            TreeViewNode parent = SelectedNode;
            TreeViewNode added = parent.AddChild(_renamedNodeName);
            parent.IsExpanded = true;
            added.IsSelected = true;
            Document.IsDirty = true;
            _renamedNodeName = null;

            if (!Document.HasFile)
            {
                RequestPersist();
            }

            _totalNodeCount = _totalNodeCount.Add(1);
            RaisePropertyChanged(nameof(TotalNodeCount));
        }

        /// <summary>
        /// 子ノードを追加できるならTrueを返す
        /// </summary>
        /// <returns>子ノードを追加できるならTrue</returns>
        private bool CanAddChild()
        {
            if (SelectedNode == null)
            {
                return false;
            }

            if (SelectedNode.Depth >= NodeDepth.MAX)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 兄弟ノードを追加する
        /// </summary>
        private void AddBrother()
        {
            if (SelectedNode == null)
            {
                return;
            }

            _renamedNodeName = null;

            var p = new DialogParameters
            {
                { nameof(RenameViewModel.EditText), string.Empty }
            };
            _dialogService.ShowDialog(nameof(RenameView), p, RenameViewClose);

            // ノード名が入力されていないなら何もしない
            if (_renamedNodeName is null)
            {
                return;
            }

            // ルートが選択されているなら Roots に追加、子なら親の Children に追加
            if (SelectedNode.Parent == null)
            {
                int index = Roots.IndexOf(SelectedNode);
                var newRoot = new TreeViewNode(_renamedNodeName);

                Roots.Insert(index + 1, newRoot);
                newRoot.IsSelected = true;
            }
            else
            {
                var parent = SelectedNode.Parent;
                int index = parent.Children.IndexOf(SelectedNode);

                TreeViewNode brother = parent.AddChild(_renamedNodeName, index);
                brother.IsSelected = true;
            }
            Document.IsDirty = true;

            if (!Document.HasFile)
            {
                RequestPersist();
            }

            _totalNodeCount = _totalNodeCount.Add(1);
            RaisePropertyChanged(nameof(TotalNodeCount));
        }

        /// <summary>
        /// 選択しているノードを上に動かす
        /// </summary>
        private void MoveUp()
        {
            if (!CanMove(-1)) return;

            var siblings = GetSiblingCollection(SelectedNode!);
            var index = siblings!.IndexOf(SelectedNode!);
            siblings.Move(index, index - 1);
            RefreshMoveCommands();

            Document.IsDirty = true;
            RequestPersist();
        }

        /// <summary>
        /// 選択しているノードを下に動かす
        /// </summary>
        private void MoveDown()
        {
            if (!CanMove(1)) return;

            var siblings = GetSiblingCollection(SelectedNode!);
            var index = siblings!.IndexOf(SelectedNode!);
            siblings.Move(index, index + 1);
            RefreshMoveCommands();

            Document.IsDirty = true;
            RequestPersist();
        }

        /// <summary>
        /// 選択しているノードを動かすことができるならTrueを返す
        /// </summary>
        /// <param name="deltaIndex">動かすインデックスの数</param>
        /// <returns>動かすことができるならTrue</returns>
        private bool CanMove(int deltaIndex)
        {
            if (SelectedNode == null)
            {
                return false;
            }

            var siblings = GetSiblingCollection(SelectedNode);
            if (siblings == null)
            {
                return false;
            }

            var index = siblings.IndexOf(SelectedNode);
            if (index < 0)
            {
                return false;
            }

            var newIndex = index + deltaIndex;
            return newIndex >= 0 && newIndex < siblings.Count;
        }

        /// <summary>
        /// ノードの名前を変更する
        /// </summary>
        private void Rename()
        {
            if (SelectedNode == null)
            {
                return;
            }

            var p = new DialogParameters
            {
                { nameof(RenameViewModel.EditText), SelectedNode.Name.Value }
            };
            _dialogService.ShowDialog(nameof(RenameView), p, RenameViewClose);

            if (_renamedNodeName is null)
            {
                return;
            }

            SelectedNode.Name = _renamedNodeName;

            Document.IsDirty = true;
            RequestPersist();
        }

        /// <summary>
        /// ノードを削除する
        /// </summary>
        private void Remove()
        {
            if (SelectedNode == null)
            {
                return;
            }

            var target = SelectedNode;
            long removeCount = target.TotalNodeCount;

            // 削除対象と同じ階層のコレクションと、その中でのインデックスを保持
            var siblings = GetSiblingCollection(target)!;
            int index = siblings.IndexOf(target);
            TreeViewNode? parent = target.Parent;

            // 先に選択を外す（削除後にSelectedItemが変になるのを避ける）
            SelectedNode = null;

            // 実際に削除
            siblings.Remove(target);

            // TreeView が空になってしまう場合はプレースホルダーを追加してそれを選択
            if (Roots.Count == 0)
            {
                var placeHolder = TreeViewNode.CreateInitialNode();
                Roots.Add(placeHolder);
                placeHolder.IsSelected = true;
                removeCount -= 1; // 1つ追加することになるので
            }
            else
            {
                TreeViewNode? nextSelection;

                if (siblings.Count > 0)
                {
                    if (index == siblings.Count)
                    {
                        // 末尾の兄弟ノードだった
                        nextSelection = siblings[index - 1];
                    }
                    else
                    {
                        // 削除したノードの下に1つ以上の兄弟ノードがある
                        nextSelection = siblings[index];
                    }
                }
                else
                {
                    // 兄弟ノードがない
                    nextSelection = parent;
                }

                if (nextSelection != null)
                {
                    nextSelection.IsSelected = true;
                }
            }

            Document.IsDirty = true;
            RequestPersist();

            _totalNodeCount = _totalNodeCount.Subtract(removeCount);
            RaisePropertyChanged(nameof(TotalNodeCount));
        }

        /// <summary>
        /// ツリーの展開状態を設定する
        /// </summary>
        /// <param name="isExpanded">展開するときはTrue</param>
        private void SetExpandedAll(bool isExpanded)
        {
            var operationLevel = TotalNodeCount.GetOperationsLevel();
            if (operationLevel == NodeCount.OperationsLevel.Warning)
            {
                string caption = isExpanded ? "すべて展開" : "すべてて折りたたむ";
                var result = _messageService.Question("ノード数が多いため、処理に時間がかかる場合があります。続行しますか？", caption);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            foreach (var root in Roots)
            {
                SetExpandedRecursive(root, isExpanded);
            }
        }

        /// <summary>
        /// 展開状態の操作ができるならTrue
        /// </summary>
        /// <returns></returns>
        private bool CanSetExpandedAll()
        {
            var operationLevel = TotalNodeCount.GetOperationsLevel();
            if (operationLevel == NodeCount.OperationsLevel.Disable)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// テキスト形式の樹形図をクリップボードにコピーする
        /// </summary>
        private void ConvertTreeToText()
        {
            var sb = new StringBuilder();
            var roots = TreeNodeMapper.ToDomain(Roots);
            foreach (var root in roots)
            {
                sb.Append(AsciiTreeBuilder.Build(root, BranchLengthValue));
            }

            _clipboardService.SetText(sb.ToString());

            _messageService.Information("クリップボードにコピーしました。");
        }

        /// <summary>
        /// 兄弟ノードを返す
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private ObservableCollection<TreeViewNode>? GetSiblingCollection(TreeViewNode node)
        {
            return node.Parent == null ? Roots : node.Parent.Children;
        }

        /// <summary>
        /// データ永続化の実行予約を行う
        /// </summary>
        private void RequestPersist()
        {
            _persistTimer.Request(() =>
            {
                var snapshot = TreeNodeMapper.ToDomain(Roots);
                _treeRepository.SaveTree(snapshot);
            });
        }

        /// <summary>
        /// ノード名編集画面を閉じた時の処理
        /// </summary>
        /// <param name="result"></param>
        private void RenameViewClose(IDialogResult result)
        {
            try
            {
                if (result.Result != ButtonResult.OK)
                {
                    _renamedNodeName = null;
                    return;
                }

                _renamedNodeName = result.Parameters.GetValue<NodeName>(nameof(RenameViewModel.EditText));
            }
            finally
            {
            }
        }

        /// <summary>
        /// 上下移動の可否を再評価する
        /// </summary>
        private void RefreshMoveCommands()
        {
            MoveUpCommand.RaiseCanExecuteChanged();
            MoveDownCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 選択中のノードが変化したときに実行される処理
        /// </summary>
        /// <param name="obj">object</param>
        private void TreeViewSelectedItemChangedExecute(object obj)
        {
            if (obj is TreeViewNode node)
            {
                SelectedNode = node;
            }
            else
            {
                SelectedNode = null;
            }
        }

        /// <summary>
        /// 再帰的にツリーの展開状態を設定する
        /// </summary>
        /// <param name="node">ノード</param>
        /// <param name="isExpanded">展開するときはTrue</param>
        private static void SetExpandedRecursive(TreeViewNode node, bool isExpanded)
        {
            node.IsExpanded = isExpanded;

            foreach (var child in node.Children)
            {
                SetExpandedRecursive(child, isExpanded);
            }
        }

        /// <summary>
        /// 新規作成
        /// </summary>
        private void CreateNew()
        {
            if (Document.IsDirty)
            {
                var result = _messageService.Question("ファイルの内容が変更されています。保存しますか？", "変更内容の保存");
                if (result == MessageBoxResult.Yes)
                {
                    // ファイルを作成していないなら、名前を付けて保存する
                    if (!Document.HasFile)
                    {
                        if (!SaveAsCore())
                        {
                            // 名前を付けて保存を中断したので、開くのも中断する
                            return;
                        }
                    }
                    else
                    {
                        // 上書き保存
                        SaveCore();
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    // 後続の処理もキャンセル
                    return;
                }
            }

            // TreeView を初期状態に戻す
            Roots.Clear();
            Roots.Add(TreeViewNode.CreateInitialNode());

            // ドキュメント状態をリセット
            Document.FilePath = string.Empty;
            Document.IsDirty = false;

            // 選択状態もクリア
            SelectedNode = Roots.FirstOrDefault();

            SetTotalNodeCount();
            RaisePropertyChanged(nameof(TotalNodeCount));
        }

        /// <summary>
        /// 開く
        /// </summary>
        private void Open()
        {
            if (Document.IsDirty)
            {
                var result = _messageService.Question("ファイルの内容が変更されています。保存しますか？", "変更内容の保存");

                // 保存する
                if (result == MessageBoxResult.Yes)
                {
                    // ファイルを作成していないなら、名前を付けて保存する
                    if (!Document.HasFile)
                    {
                        if (!SaveAsCore())
                        {
                            // 名前を付けて保存を中断したので、開くのも中断する
                            return;
                        }
                    }
                    else
                    {
                        // 上書き保存
                        SaveCore();
                    }
                }
                else if (result == MessageBoxResult.Cancel) 
                {
                    // 後続の処理もキャンセル
                    return;
                }
            }

            // ファイル読み込みのダイアログを開く
            var dlg = new OpenFileDialog
            {
                Title = "開く",
                Filter = "JSON files (*.json)|*.json",
                DefaultExt = ".json",
                AddExtension = true,
                CheckFileExists = true,
                Multiselect = false
            };

            // 既にファイルを開いているなら、その場所を初期ディレクトリにする
            if (Document.HasFile)
            {
                var dir = Path.GetDirectoryName(Document.FilePath);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    dlg.InitialDirectory = dir;
                }
            }

            var ok = dlg.ShowDialog();
            if (ok != true)
            {
                return;
            }

            string path = dlg.FileName;
            var store = Factories.CreateTreeFile();

            IReadOnlyList<ITreeNode> roots;

            try
            {
                roots = store.LoadFrom(path);
            }
            catch (TreeRestoreException ex)
            {
                _messageService.Error(ex.Message, ex.Detail);
                return;
            }
            catch (Exception ex)
            {
                _messageService.Error(ex.Message);
                return;
            }
            
            var uiRoots = TreeNodeMapper.ToUi(roots);

            Roots.Clear();
            foreach (var r in uiRoots)
            {
                Roots.Add(r);
            }

            // ドキュメント状態更新
            Document.FilePath = path;
            Document.IsDirty = false;

            // 選択状態
            SelectedNode = Roots.FirstOrDefault();

            SetTotalNodeCount();
            RaisePropertyChanged(nameof(TotalNodeCount));
        }

        /// <summary>
        /// 上書き保存
        /// </summary>
        private void Save()
        {
            // ファイルを作成していないなら、名前を付けて保存する
            if (!Document.HasFile)
            {
                SaveAsCore();
                return;
            }

            SaveCore();
            return;
        }

        /// <summary>
        /// 上書き保存処理
        /// </summary>
        /// <returns>正常に保存したらTrue, </returns>
        private void SaveCore()
        {
            var roots = TreeNodeMapper.ToDomain(Roots);
            var store = Factories.CreateTreeFile();
            store.SaveTo(Document.FilePath, roots);

            // 保存済みにする
            Document.IsDirty = false;
        }

        /// <summary>
        /// 名前を付けて保存
        /// </summary>
        /// <returns>正常に実行されたらTrue, 途中で中断されたらFalse</returns>
        private void SaveAs()
        {
            SaveAsCore();
        }

        /// <summary>
        /// 名前を付けて保存処理
        /// </summary>
        /// <returns></returns>
        private bool SaveAsCore()
        {
            var dlg = new SaveFileDialog
            {
                Title = "名前を付けて保存",
                Filter = "JSON files (*.json)|*.json",
                DefaultExt = ".json",
                AddExtension = true,
                FileName = "tree.json",
                OverwritePrompt = true
            };

            // 既にファイルを持っているなら、その場所・名前を初期値にする
            if (Document.HasFile)
            {
                dlg.FileName = System.IO.Path.GetFileName(Document.FilePath);
                var dir = System.IO.Path.GetDirectoryName(Document.FilePath);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    dlg.InitialDirectory = dir;
                }
            }

            bool? ok = dlg.ShowDialog();
            if (ok != true)
            {
                // 中断された
                return false;
            }

            // 選択された保存先
            Document.FilePath = dlg.FileName;

            // 保存
            SaveCore();

            // 保存済みにする
            Document.IsDirty = false;
            return true;
        }

        /// <summary>
        /// 終了
        /// </summary>
        private void Quit()
        {
            // 後処理は MainWindowViewModel のコードビハインドのメソッド OnClosing でおこなう
            // 画面右上のバツ印を押下したときと共通処理にしたいので
            Application.Current.MainWindow?.Close();
        }

        /// <summary>
        /// アプリケーションのタイトルを更新する
        /// </summary>
        /// <remarks>
        /// Document.IsDirty が変化すると発火するようになっている。
        /// ViewModel のコンストラクタを参照。
        /// </remarks>
        private void UpdateAppTitle()
        {
            if (!Document.HasFile)
            {
                Title = APP_TITLE;
                return;
            }

            var fileName = System.IO.Path.GetFileName(Document.FilePath);
            var dirtyMark = Document.IsDirty ? " *" : string.Empty;

            Title = $"{APP_TITLE} - {fileName}{dirtyMark}";
        }

        /// <summary>
        /// アプリを閉じる操作をしたときに確認を取る
        /// </summary>
        /// <returns></returns>
        internal bool ConfirmClose()
        {
            // TBD: Yesだったら保存処理を入れる
            // ファイルを持っていて未保存の場合は破棄されてしまうので確認する
            if (Document.HasFile && Document.IsDirty)
            {
                var result = _messageService.Question("ファイルの内容が変更されています。保存しますか？", "変更内容の保存");
                if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }
                return result == MessageBoxResult.OK;
            }

            return true;
        }

        /// <summary>
        /// ユーザの設定を保存する
        /// </summary>
        internal void SaveUserPreferences()
        {
            UserPreferenceEntity userPreferences = new UserPreferenceEntity(_branchLength);
            _userPreferencesRepository.Save(userPreferences);
        }

        /// <summary>
        /// 総ノード数をカウントする
        /// 計算量: O(N)
        /// </summary>
        private void SetTotalNodeCount()
        {
            long totalCount = 0;
            foreach (var root in Roots)
            {
                totalCount += root.TotalNodeCount;
            }
            _totalNodeCount = new NodeCount(totalCount);
        }

        #endregion
    }
}
