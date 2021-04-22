using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public sealed class PathNode<TItem> : PathNodeBase<PathNode<TItem>, TItem>
    {
        public PathNode(string rootNodeName, TItem item) :
            base(rootNodeName, item)
        {
        }
    }
}
