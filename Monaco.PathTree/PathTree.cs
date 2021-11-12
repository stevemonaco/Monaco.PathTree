using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree;

/// <summary>
/// Tree that can store nodes which are accessible by paths
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TItem"></typeparam>
public sealed class PathTree<TNode, TItem> : PathTreeBase<TNode, TItem>
    where TNode : PathNodeBase<TNode, TItem>
{
    public PathTree(TNode root) :
        base(root)
    {
    }
}
