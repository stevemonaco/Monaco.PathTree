using System.Collections.Generic;

namespace Monaco.PathTree.Abstractions
{
    public interface IPathNode<TNode, TItem, TMetadata>
        where TNode : IPathNode<TNode, TItem, TMetadata>
    {
        IEnumerable<TItem> ChildItems { get; }
        IEnumerable<TNode> ChildNodes { get; }
        TItem Item { get; set; }
        TMetadata Metadata { get; set; }
        string Name { get; }
        TNode Parent { get; set; }
        string PathKey { get; }
        IEnumerable<string> Paths { get; }

        TNode AddChild(string nodeName, TItem item, TMetadata metadata = default);
        void AttachChildNode(TNode node);
        bool ContainsChildNode(string name);
        TNode DetachChildNode(string nodeName);
        void RemoveChildNode(string nodeName);
        void Rename(string name);
        void RenameChild(string name, string newName);
        bool TryGetChildNode(string name, out TNode node);
    }
}