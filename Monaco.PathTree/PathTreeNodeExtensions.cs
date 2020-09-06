using System;
using System.Collections.Generic;
using System.Text;

namespace Monaco.PathTree
{
    /// <summary>
    /// Extension methods that allow iteration over the IPathTreeNode
    /// </summary>
    /// <returns></returns>
    /// <remarks>Idea adapted from https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html 
    /// Implementation adapted from https://blogs.msdn.microsoft.com/wesdyer/2007/03/23/all-about-iterators/
    /// </remarks>
    public static class PathTreeNodeExtensions
    {
        public static IEnumerable<IPathTreeNode<T>> SelfAndAncestors<T>(this IPathTreeNode<T> node)
        {
            IPathTreeNode<T> nodeVisitor = node;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        public static IEnumerable<IPathTreeNode<T>> Ancestors<T>(this IPathTreeNode<T> node)
        {
            IPathTreeNode<T> nodeVisitor = node.Parent;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        public static IEnumerable<IPathTreeNode<T>> SelfAndDescendantsDepthFirst<T>(this IPathTreeNode<T> node)
        {
            var nodeStack = new Stack<IPathTreeNode<T>>();

            nodeStack.Push(node);

            while (nodeStack.Count > 0)
            {
                var popNode = nodeStack.Pop();
                yield return popNode;
                foreach (var child in popNode.Children)
                    nodeStack.Push(child);
            }
        }

        public static IEnumerable<IPathTreeNode<T>> SelfAndDescendantsBreadthFirst<T>(this IPathTreeNode<T> node)
        {
            var nodeQueue = new Queue<IPathTreeNode<T>>();

            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                var dequeueNode = nodeQueue.Dequeue();
                yield return dequeueNode;
                foreach (var child in dequeueNode.Children)
                    nodeQueue.Enqueue(child);
            }
        }

        public static IEnumerable<IPathTreeNode<T>> DescendantsDepthFirst<T>(this IPathTreeNode<T> node)
        {
            var nodeStack = new Stack<IPathTreeNode<T>>(node.Children);

            while (nodeStack.Count > 0)
            {
                var popNode = nodeStack.Pop();
                yield return popNode;
                foreach (var child in popNode.Children)
                    nodeStack.Push(child);
            }
        }

        public static IEnumerable<IPathTreeNode<T>> DescendantsBreadthFirst<T>(this IPathTreeNode<T> node)
        {
            var nodeQueue = new Queue<IPathTreeNode<T>>();

            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                var dequeueNode = nodeQueue.Dequeue();
                yield return dequeueNode;
                foreach (var child in dequeueNode.Children)
                    nodeQueue.Enqueue(child);
            }
        }
    }
}
