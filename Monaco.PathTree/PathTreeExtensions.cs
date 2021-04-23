using Monaco.PathTree.Abstractions;
using System.Collections.Generic;

namespace Monaco.PathTree
{
    /// <summary>
    /// Extension methods that allow iteration over an IPathTree
    /// </summary>
    public static class PathTreeExtensions
    {
        /// <summary>
        /// Enumerates the tree in a depth-first traversal
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="tree">Tree to traverse</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> EnumerateDepthFirst<TNode, TItem>
            (this IPathTree<TNode, TItem> tree)
            where TNode : IPathNode<TNode, TItem>
        {
            return tree.Root.SelfAndDescendantsDepthFirst<TNode, TItem>();
        }

        /// <summary>
        /// Enumerates the tree in a breadth-first traversal
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="tree">Tree to traverse</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> EnumerateBreadthFirst<TNode, TItem>
            (this IPathTree<TNode, TItem> tree)
            where TNode : IPathNode<TNode, TItem>
        {
            return tree.Root.SelfAndDescendantsBreadthFirst<TNode, TItem>();
        }

        /// <summary>
        /// Traverses the tree to count the number of items contained within
        /// </summary>
        /// <returns>Number of items</returns>
        public static int Count<TNode, TItem>
            (this IPathTree<TNode, TItem> tree)
            where TNode : IPathNode<TNode, TItem>
        {
            int nodeCount = 0;
            var nodeStack = new Stack<TNode>();

            nodeStack.Push(tree.Root);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                nodeCount++;

                foreach (var child in node.ChildNodes)
                    nodeStack.Push(child);
            }

            return nodeCount;
        }
    }
}
