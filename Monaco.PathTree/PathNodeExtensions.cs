using Monaco.PathTree.Abstractions;
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
        public static IEnumerable<TNode> SelfAndAncestors<TNode, TItem>(this TNode node)
            where TNode : IPathNode<TNode, TItem>
        {
            var nodeVisitor = node;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

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

        public static IEnumerable<TNode> DescendantsDepthFirst<TNode, TItem, TMetadata>(this TNode node)
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

        public static IEnumerable<TNode> DescendantsBreadthFirst<TNode, TItem, TMetadata>(this TNode node)
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
