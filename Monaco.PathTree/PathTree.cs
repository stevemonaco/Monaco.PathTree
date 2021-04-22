using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public sealed class PathTree<TNode, TItem> : PathTreeBase<TNode, TItem>
        where TNode : PathNodeBase<TNode, TItem>
    {
        public PathTree(TNode root) :
            base(root)
        {
        }
    }
}
