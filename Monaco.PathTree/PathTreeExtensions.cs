using System.Collections.Generic;

namespace Monaco.PathTree
{
    public static class PathTreeExtensions
    {
        public static IEnumerable<IPathTreeNode<T>> EnumerateDepthFirst<T>(this IPathTree<T> tree) =>
            tree.Root.SelfAndDescendantsDepthFirst();            

        public static IEnumerable<IPathTreeNode<T>> EnumerateBreadthFirst<T>(this IPathTree<T> tree) =>
            tree.Root.SelfAndDescendantsBreadthFirst();
    }
}
