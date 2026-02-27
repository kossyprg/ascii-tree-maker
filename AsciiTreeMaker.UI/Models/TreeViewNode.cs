using AsciiTreeMaker.Domain.ValueObjects;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AsciiTreeMaker.UI.Models
{
    /// <summary>
    /// ツリービュー用モデル
    /// </summary>
    public class TreeViewNode : BindableBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ノードの名前</param>
        /// <param name="parent">親ノード</param>
        public TreeViewNode(NodeName name, TreeViewNode? parent = null)
        {
            _name = name;
            Parent = parent;
            Children = new ObservableCollection<TreeViewNode>();
        }

        /// <summary>
        /// ノードの階層
        /// ルートノードの階層を 1 とする
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = 1;
                var node = Parent;
                while (node != null)
                {
                    depth++;
                    node = node.Parent;
                }
                return depth;
            }
        }

        /// <summary>
        /// このノード + 子孫ノードの総数
        /// </summary>
        /// <remarks>
        /// 計算量: O(N)
        /// </remarks>
        public long TotalNodeCount
        {
            get
            {
                long count = 1; // 自分自身
                foreach (var child in Children)
                {
                    count += child.TotalNodeCount;
                }
                return count;
            }
        }

        /// <summary>
        /// ノード名
        /// </summary>
        private NodeName _name;

        /// <summary>
        /// ノード名
        /// </summary>
        public NodeName Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// 展開されていればTrue
        /// </summary>
        private bool _isExpanded;

        /// <summary>
        /// 展開されていればTrue
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        /// <summary>
        /// 選択されていればTrue
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// 選択されていればTrue
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        /// <summary>
        /// 子ノード
        /// </summary>
        public ObservableCollection<TreeViewNode> Children { get; }

        /// <summary>
        /// 親ノード
        /// </summary>
        public TreeViewNode? Parent { get; }

        /// <summary>
        /// 子ノードを追加する
        /// </summary>
        /// <param name="name">子ノードの名前</param>
        /// <param name="targetNodeIndex">追加する子ノードの場所を表すインデックス</param>
        /// <returns></returns>
        public TreeViewNode AddChild(NodeName? name, int targetNodeIndex = -1)
        {
            if (name is null)
            {
                name = NodeName.GetInitialNodeName();
            }

            var child = new TreeViewNode(name, parent: this);

            if (targetNodeIndex < 0 || targetNodeIndex >= Children.Count)
            {
                Children.Add(child);
            }
            else
            {
                Children.Insert(targetNodeIndex + 1, child);
            }

            return child;
        }

        /// <summary>
        /// 初期ノードを返す
        /// </summary>
        /// <returns></returns>
        public static TreeViewNode CreateInitialNode()
        {
            return new TreeViewNode(NodeName.GetInitialNodeName(), null);
        }
    }
}
