using System.Collections.Generic;

namespace Monaco.PathTree.Abstractions
{
    public interface IPathNode<TNode, TItem>
        where TNode : IPathNode<TNode, TItem>
    {
        IEnumerable<TItem> ChildItems { get; }
        IEnumerable<TNode> ChildNodes { get; }
        TItem Item { get; set; }

        string Name { get; }
        TNode? Parent { get; set; }

        void AttachChildNode(TNode node);
        bool ContainsChildNode(string childName);

        void Detach();
        TNode DetachChildNode(string childName);

        void RemoveChildNode(string childName);
        void Rename(string name);
        void RenameChild(string childName, string newName);
        bool TryGetChildNode(string childName, out TNode? node);
    }
}