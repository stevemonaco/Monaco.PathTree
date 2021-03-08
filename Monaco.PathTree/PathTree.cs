using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public sealed class PathTree<TNode, TItem, TMetadata> : PathTreeBase<TNode, TItem, TMetadata>
        where TNode : PathNodeBase<TNode, TItem, TMetadata>
    {
        public PathTree(TNode root) :
            base(root)
        {
        }
    }
}
