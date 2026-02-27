using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;
using System.Collections.Generic;

namespace AsciiTreeMaker.UI.Models
{
    internal sealed class TreeNodeAdapter : ITreeNode
    {
        public TreeNodeAdapter(NodeName name, IReadOnlyList<ITreeNode> children)
        {
            Name = name;
            Children = children;
        }

        public NodeName Name { get; }
        public IReadOnlyList<ITreeNode> Children { get; }

    }
}
