using System.Collections.Generic;
using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    /// <summary>
    /// Extension methods that allow iteration over IPathNode
    /// </summary>
    /// <remarks>
    /// Some ideas and implementation adapted from:
    /// https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html 
    /// https://blogs.msdn.microsoft.com/wesdyer/2007/03/23/all-about-iterators/
    /// </remarks>
    public static class PathNodeExtensions
    {
        /// <summary>
        /// Enumerates the sequence of nodes starting from itself up to the root node
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="node">Node to enumerate</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> SelfAndAncestors<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            TNode? nodeVisitor = node;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        /// <summary>
        /// Enumerates the sequence of nodes from its parent node up to the root node
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="node">Node to enumerate</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> Ancestors<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            var nodeVisitor = node.Parent;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        /// <summary>
        /// Enumerates all child nodes and the specified node in a depth-first traversal
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="node">Node to enumerate</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> SelfAndDescendantsDepthFirst<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            var nodeStack = new Stack<TNode>();

            nodeStack.Push(node);

            while (nodeStack.Count > 0)
            {
                var popNode = nodeStack.Pop();
                yield return popNode;
                foreach (var child in popNode.ChildNodes)
                    nodeStack.Push(child);
            }
        }

        /// <summary>
        /// Enumerates all child nodes and the specified node in a breadth-first traversal
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="node">Node to enumerate</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> SelfAndDescendantsBreadthFirst<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            var nodeQueue = new Queue<TNode>();

            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                var dequeueNode = nodeQueue.Dequeue();
                yield return dequeueNode;
                foreach (var child in dequeueNode.ChildNodes)
                    nodeQueue.Enqueue(child);
            }
        }

        /// <summary>
        /// Enumerates all child nodes in a depth-first traversal
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="node">Node to enumerate</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> DescendantsDepthFirst<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            var nodeStack = new Stack<TNode>(node.ChildNodes);

            while (nodeStack.Count > 0)
            {
                var popNode = nodeStack.Pop();
                yield return popNode;
                foreach (var child in popNode.ChildNodes)
                    nodeStack.Push(child);
            }
        }

        /// <summary>
        /// Enumerates all child nodes in a breadth-first traversal
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="node">Node to enumerate</param>
        /// <returns>Sequence of nodes</returns>
        public static IEnumerable<TNode> DescendantsBreadthFirst<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            var nodeQueue = new Queue<TNode>();

            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                var dequeueNode = nodeQueue.Dequeue();
                yield return dequeueNode;
                foreach (var child in dequeueNode.ChildNodes)
                    nodeQueue.Enqueue(child);
            }
        }
    }
}
