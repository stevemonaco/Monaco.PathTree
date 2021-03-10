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
        TNode Parent { get; set; }

        void AttachChildNode(TNode node);
        bool ContainsChildNode(string name);

        void Detach();
        TNode DetachChildNode(string nodeName);

        void RemoveChildNode(string nodeName);
        void Rename(string name);
        void RenameChild(string name, string newName);
        bool TryGetChildNode(string name, out TNode node);
    }
}