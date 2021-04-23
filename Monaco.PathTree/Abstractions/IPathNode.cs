using System.Collections.Generic;

namespace Monaco.PathTree.Abstractions
{
    public interface IPathNode<TNode, TItem>
        where TNode : IPathNode<TNode, TItem>
    {
        IEnumerable<TItem> ChildItems { get; }
        IEnumerable<TNode> ChildNodes { get; }

        /// <summary>
        /// Item contained within the node
        /// </summary>
        TItem Item { get; set; }

        /// <summary>
        /// Name of the node
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parent node
        /// </summary>
        TNode? Parent { get; set; }

        /// <summary>
        /// Attaches an existing node as a child
        /// </summary>
        /// <param name="node">Node to attach</param>
        void AttachChildNode(TNode node);

        /// <summary>
        /// Tries to get a child node by name
        /// </summary>
        /// <param name="childName">Name of child node to get</param>
        /// <param name="node"></param>
        /// <returns>True if found, false if not found</returns>
        bool TryGetChildNode(string childName, out TNode? node);

        /// <summary>
        /// Determines if this node contains a child with the specified name
        /// </summary>
        /// <param name="childName">Name of child node</param>
        /// <returns>True if contained, false if not contained</returns>
        bool ContainsChildNode(string childName);

        /// <summary>
        /// Detaches this node from its parent, if parented
        /// </summary>
        void Detach();

        /// <summary>
        /// Detaches a child node by name
        /// </summary>
        /// <param name="childName">Name of the node to detach</param>
        /// <returns></returns>
        TNode DetachChildNode(string childName);

        /// <summary>
        /// Removes a child node by name
        /// </summary>
        /// <param name="childName">Name of the node to remove</param>
        void RemoveChildNode(string childName);

        /// <summary>
        /// Renames this node
        /// </summary>
        /// <param name="name">New name</param>
        void Rename(string name);

        /// <summary>
        /// Renames a child node
        /// </summary>
        /// <param name="childName">Name of the existing child node</param>
        /// <param name="newName">New name</param>
        void RenameChild(string childName, string newName);
    }
}