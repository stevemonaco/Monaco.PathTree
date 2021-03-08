using Monaco.PathTree.Abstractions;
using System.Collections.Generic;

namespace Monaco.PathTree
{
    public static class PathTreeExtensions
    {
        public static IEnumerable<TNode> EnumerateDepthFirst<TNode, TItem, TMetadata>
            (this IPathTree<TNode, TItem, TMetadata> tree)
            where TNode : IPathNode<TNode, TItem, TMetadata>
        {
            return tree.Root.SelfAndDescendantsDepthFirst<TNode, TItem, TMetadata>();
        }

        public static IEnumerable<TNode> EnumerateBreadthFirst<TNode, TItem, TMetadata>
            (this IPathTree<TNode, TItem, TMetadata> tree)
            where TNode : IPathNode<TNode, TItem, TMetadata>
        {
            return tree.Root.SelfAndDescendantsBreadthFirst<TNode, TItem, TMetadata>();
        }
    }
}
