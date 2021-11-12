using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree;

/// <summary>
/// Simple node type that stores an item and is accessed by paths
/// </summary>
/// <typeparam name="TItem">Type of item stored within the node</typeparam>
public sealed class PathNode<TItem> : PathNodeBase<PathNode<TItem>, TItem>
{
    public PathNode(string rootNodeName, TItem item) :
        base(rootNodeName, item)
    {
    }
}
