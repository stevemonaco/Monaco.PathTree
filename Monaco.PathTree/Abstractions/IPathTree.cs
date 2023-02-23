using Monaco.PathTree.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Monaco.PathTree;

public interface IPathTree<TNode, TItem>
    where TNode : IPathNode<TNode, TItem>
{
    /// <summary>
    /// Root of the tree
    /// </summary>
    TNode Root { get; set; }

    /// <summary>
    /// Excludes the root item name from paths
    /// </summary>
    bool ExcludeRootFromPath { get; set; }

    /// <summary>
    /// Valid string separators for paths
    /// </summary>
    string[] PathSeparators { get; set; }

    /// <summary>
    /// Attaches the node as the specified path if the parent exists and renames the node if necessary
    /// </summary>
    /// <param name="path">The full path associated with the node</param>
    /// <param name="node">The node to be attached</param>
    void AttachNodeAsPath(string path, TNode node);

    /// <summary>
    /// Attaches the node as a child of the specified path
    /// </summary>
    /// <param name="path">The full path associated with the parent</param>
    /// <param name="node">Node to attach</param>
    void AttachNodeToPath(string path, TNode node);

    /// <summary>
    /// Tries to get an existing item stored at the specified path
    /// </summary>
    /// <param name="path">The full path associated with the item</param>
    /// <param name="item"></param>
    /// <returns>True if successful, false if failed</returns>
    bool TryGetItem(string path, [MaybeNullWhen(false)] out TItem item);

    /// <summary>
    /// Tries to get an existing item of a specific type stored at the specified path
    /// </summary>
    /// <typeparam name="TDerivedItem">Type of the item</typeparam>
    /// <param name="path">The full path associated with the item</param>
    /// <param name="item"></param>
    /// <returns>True if successful, false if failed</returns>
    bool TryGetItem<U>(string path, [MaybeNullWhen(false)] out U item) where U : TItem;

    /// <summary>
    /// Tries to get the node contained at the specified location
    /// </summary>
    /// <param name="path">The full path associated with the item</param>
    /// <param name="node"></param>
    /// <returns>True if found, false if not found</returns>
    bool TryGetNode(string path, [MaybeNullWhen(false)] out TNode node);

    /// <summary>
    /// Removes the node at the specified location
    /// </summary>
    /// <param name="path">The full path associated with the item</param>
    void RemoveNode(string path);

    /// <summary>
    /// Creates a path key which can be used to access the node
    /// </summary>
    /// <param name="node">The node to create the key to</param>
    /// <returns>A key which can be used to lookup the node</returns>
    string CreatePathKey(TNode node);

    /// <summary>
    /// Creates a path key which can be used to access the node
    /// </summary>
    /// <param name="node">The node to create the key to</param>
    /// <param name="separator">The separator to use between each node level of the tree when building the key</param>
    /// <returns>A key which can be used to lookup the node</returns>
    string CreatePathKey(TNode node, string separator);
}
