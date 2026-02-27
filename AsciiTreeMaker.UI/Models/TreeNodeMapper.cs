using AsciiTreeMaker.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AsciiTreeMaker.UI.Models
{
    public static class TreeNodeMapper
    {
        /// <summary>
        /// UI層のツリーをDomain層のツリーへ変換
        /// </summary>
        /// <param name="node">UI層のツリー</param>
        /// <returns>Domain層のツリー</returns>
        public static ITreeNode ToDomain(TreeViewNode node)
        {
            var children = node.Children
                .Select(ToDomain)
                .ToList()
                .AsReadOnly();

            return new TreeNodeAdapter(node.Name, children);
        }

        /// <summary>
        /// UI層のツリーをDomain層のツリーへ変換
        /// </summary>
        /// <param name="nodes">UI層のツリー</param>
        /// <returns></returns>
        public static IReadOnlyList<ITreeNode> ToDomain(IEnumerable<TreeViewNode> nodes)
        {
            return nodes
                .Select(ToDomain)
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Domain層のツリーをUI層のツリーへ変換
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static TreeViewNode ToUi(ITreeNode node, TreeViewNode? parent = null)
        {
            var ui = new TreeViewNode(node.Name, parent);

            foreach (var child in node.Children)
            {
                ui.Children.Add(ToUi(child, ui)); // 再帰で Parent も正しく設定
            }

            return ui;
        }

        /// <summary>
        /// Domain層のツリーをUI層のツリーへ変換
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static IReadOnlyList<TreeViewNode> ToUi(IEnumerable<ITreeNode> nodes)
        {
            return nodes.Select(n => ToUi(n)).ToList();
        }
    }

}
