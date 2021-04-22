using Monaco.PathTree.Abstractions;
using System.Collections.Generic;

namespace Monaco.PathTree
{
    public static class PathTreeExtensions
    {
        public static IEnumerable<TNode> EnumerateDepthFirst<TNode, TItem>
            (this IPathTree<TNode, TItem> tree)
            where TNode : IPathNode<TNode, TItem>
        {
            return tree.Root.SelfAndDescendantsDepthFirst<TNode, TItem>();
        }

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
