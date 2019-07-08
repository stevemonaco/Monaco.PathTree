using System;
using System.Collections.Generic;
using System.IO;

namespace Monaco.PathTree
{
    public class PathTree<T> : IPathTree<T>
    {
        private IPathTreeNode<T> root = new PathTreeNode<T>($"{nameof(PathTree<T>)}.RootNode", default);
        public char[] PathSeparators { get; set; } = new char[] { '\\', '/' };

        /// <summary>
        /// Adds the item to the specified path if the parent exists
        /// </summary>
        /// <param name="path">The path associated with the item</param>
        /// <param name="value">The item</param>
        public void Add(string path, T value)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(Add)}: parameter '{nameof(path)}' was null or empty");

            var nodeNames = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);

            if(nodeNames.Length == 1) // Add to root
            {
                root.AddChild(nodeNames[0], value);
                return;
            }

            IPathTreeNode<T> nodeVisitor = root;

            for(int i = 0; i < nodeNames.Length - 1; i++)
            {
                if(!nodeVisitor.TryGetChild(nodeNames[i], out nodeVisitor))
                    throw new KeyNotFoundException($"{nameof(TryGetValue)}: could not find {nameof(path)} '{path}'");
            }

            nodeVisitor.AddChild(nodeNames[nodeNames.Length - 1], value);
        }

        public bool TryGetValue(string path, out T value)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetValue)}: parameter '{nameof(path)}' was null or empty");

            if (TryGetNode(path, out var node))
            {
                value = node.Value;
                return true;
            }

            value = default(T);
            return false;
        }

        public bool TryGetNode(string path, out IPathTreeNode<T> node)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetNode)}: parameter '{nameof(path)}' was null or empty");

            var nodeNames = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);

            var nodeVisitor = root;
            IPathTreeNode<T> nextNode = default;

            foreach (var name in nodeNames)
            {
                if (!nodeVisitor.TryGetChild(name, out nextNode))
                {
                    node = default(PathTreeNode<T>);
                    return false;
                }
                nodeVisitor = nextNode;
            }

            node = nextNode;
            return true;
        }

        public void RemoveNode(string absolutePath)
        {
            if(string.IsNullOrWhiteSpace(absolutePath))
                throw new ArgumentException();

            if (!TryGetNode(absolutePath, out var removeNode))
                throw new KeyNotFoundException($"RemoveNode was unable to locate {nameof(absolutePath)}");

            if (removeNode.Parent is null)
                throw new NullReferenceException();

            removeNode.Parent.RemoveChild(removeNode.Name);
        }

        /// <summary>
        /// Allows iteration over the PathTrie
        /// </summary>
        /// <returns></returns>
        /// <remarks>Idea adapted from https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html 
        /// Implementation adapted from https://blogs.msdn.microsoft.com/wesdyer/2007/03/23/all-about-iterators/
        /// </remarks>
        public IEnumerable<IPathTreeNode<T>> EnumerateDepthFirst() => root.DescendantsDepthFirst();

        public IEnumerable<IPathTreeNode<T>> EnumerateBreadthFirst() => root.DescendantsDepthFirst();

        public IEnumerable<T> DescendantsDepthFirst(string nodePath = "")
        {
            var nodeStack = new Stack<IPathTreeNode<T>>();

            foreach (var node in root.Children())
                nodeStack.Push(node);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                yield return node.Value;

                foreach (var child in node.Children())
                    nodeStack.Push(child);
            }
        }

        public int Count()
        {
            int nodeCount = 0;
            var nodeStack = new Stack<IPathTreeNode<T>>();

            nodeStack.Push(root);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                nodeCount++;

                foreach (var child in node.Children())
                    nodeStack.Push(child);
            }

            return nodeCount - 1;
        }

        public void AttachNode(string path)
        {
            throw new NotImplementedException();
        }
    }
}
