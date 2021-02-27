using System.Collections.Generic;

namespace Monaco.PathTree
{
    public static class PathTreeExtensions
    {
        public static IEnumerable<PathNode<TItem, TMetadata>> EnumerateDepthFirst<TItem, TMetadata>
            (this IPathTree<TItem, TMetadata> tree) =>
            tree.Root.SelfAndDescendantsDepthFirst();

        public static IEnumerable<PathNode<TItem, TMetadata>> EnumerateBreadthFirst<TItem, TMetadata>
            (this IPathTree<TItem, TMetadata> tree) =>
            tree.Root.SelfAndDescendantsBreadthFirst();
    }
}
