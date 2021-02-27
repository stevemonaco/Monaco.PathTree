using System;
using System.Collections.Generic;
using System.Text;

namespace Monaco.PathTree
{
    /// <summary>
    /// Extension methods that allow iteration over PathNode
    /// </summary>
    /// <returns></returns>
    /// <remarks>Idea adapted from https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html 
    /// Implementation adapted from https://blogs.msdn.microsoft.com/wesdyer/2007/03/23/all-about-iterators/
    /// </remarks>
    public static class PathNodeExtensions
    {
        public static IEnumerable<PathNode<TItem, TMetadata>> SelfAndAncestors<TItem, TMetadata>(this PathNode<TItem, TMetadata> node)
        {
            var nodeVisitor = node;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        public static IEnumerable<PathNode<TItem, TMetadata>> Ancestors<TItem, TMetadata>(this PathNode<TItem, TMetadata> node)
        {
            var nodeVisitor = node.Parent;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        public static IEnumerable<PathNode<TItem, TMetadata>> SelfAndDescendantsDepthFirst<TItem, TMetadata>(this PathNode<TItem, TMetadata> node)
        {
            var nodeStack = new Stack<PathNode<TItem, TMetadata>>();

            nodeStack.Push(node);

            while (nodeStack.Count > 0)
            {
                var popNode = nodeStack.Pop();
                yield return popNode;
                foreach (var child in popNode.ChildNodes)
                    nodeStack.Push(child);
            }
        }

        public static IEnumerable<PathNode<TItem, TMetadata>> SelfAndDescendantsBreadthFirst<TItem, TMetadata>(this PathNode<TItem, TMetadata> node)
        {
            var nodeQueue = new Queue<PathNode<TItem, TMetadata>>();

            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                var dequeueNode = nodeQueue.Dequeue();
                yield return dequeueNode;
                foreach (var child in dequeueNode.ChildNodes)
                    nodeQueue.Enqueue(child);
            }
        }

        public static IEnumerable<PathNode<TItem, TMetadata>> DescendantsDepthFirst<TItem, TMetadata>(this PathNode<TItem, TMetadata> node)
        {
            var nodeStack = new Stack<PathNode<TItem, TMetadata>>(node.ChildNodes);

            while (nodeStack.Count > 0)
            {
                var popNode = nodeStack.Pop();
                yield return popNode;
                foreach (var child in popNode.ChildNodes)
                    nodeStack.Push(child);
            }
        }

        public static IEnumerable<PathNode<TItem, TMetadata>> DescendantsBreadthFirst<TItem, TMetadata>(this PathNode<TItem, TMetadata> node)
        {
            var nodeQueue = new Queue<PathNode<TItem, TMetadata>>();

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
