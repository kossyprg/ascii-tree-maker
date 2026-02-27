using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Domain.ValueObjects;

namespace AsciiTreeMaker.Infrastructure.Json
{
    internal sealed class JsonTreeNodeReadOnly : ITreeNode
    {
        public JsonTreeNodeReadOnly(NodeName name, IReadOnlyList<ITreeNode> children)
        {
            Name = name;
            Children = children;
        }

        public IReadOnlyList<ITreeNode> Children { get; }

        public NodeName Name { get; }
    }
}
