using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monaco.PathTree
{
    public class PathTree<T> : IPathTree<T>
    {
        public IPathTreeNode<T> Root { get; set; }
        public char[] PathSeparators { get; set; } = new char[] { '\\', '/' };

        public PathTree() { }

        public PathTree(string rootName, T root)
        {
            var rootNode = new PathTreeNode<T>(rootName, root);
            Root = rootNode;
        }

        /// <summary>
        /// Adds the item to the specified path if the parent exists
        /// </summary>
        /// <param name="path">The path associated with the item</param>
        /// <param name="value">The item</param>
        public void Add(string path, T value)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(Add)}: parameter '{nameof(path)}' was null or empty");

            var parent = ResolveParent(path);

            if (parent is null)
                throw new KeyNotFoundException($"{nameof(TryGetValue)}: could not find {nameof(path)} '{path}'");

            var nodeName = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries).Last();
            parent.AddChild(nodeName, value);
        }

        public bool TryGetValue(string path, out T value)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetValue)}: parameter '{nameof(path)}' was null or empty");

            var node = ResolveNode(path);
            
            if (node is object)
            {
                value = node.Value;
                return true;
            }

            value = default(T);
            return false;
        }

        public bool TryGetValue<U>(string path, out U value) where U : T
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetValue)}: parameter '{nameof(path)}' was null or empty");

            var node = ResolveNode(path);

            if (node is object)
            {
                value = (U)node.Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetNode(string path, out IPathTreeNode<T> node)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetNode)}: parameter '{nameof(path)}' was null or empty");

            node = ResolveNode(path);

            return node is null;
        }

        public bool TryGetNode<TDerived>(string path, out IPathTreeNode<TDerived> node) where TDerived : T
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetNode)}: parameter '{nameof(path)}' was null or empty");

            var resolvedNode = ResolveNode(path);

            if (resolvedNode is object)
            {
                node = (IPathTreeNode<TDerived>) resolvedNode;
                return true;
            }

            node = default;
            return false;
        }

        public void RemoveNode(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();

            var removeNode = ResolveNode(path);

            if (removeNode is null)
                throw new KeyNotFoundException($"RemoveNode was unable to locate {nameof(path)}");

            if (removeNode.Parent is null)
                throw new NullReferenceException();

            removeNode.Parent.RemoveChild(removeNode.Name);
        }

        private IPathTreeNode<T> ResolveNode(string absolutePath)
        {
            var nodeNames = absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            return Resolve(nodeNames);
        }

        private IPathTreeNode<T> ResolveParent(string absolutePath)
        {
            var nodeNames = absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            var parentNodeNames = nodeNames.Take(nodeNames.Length - 1).ToList();

            return Resolve(parentNodeNames);
        }

        private IPathTreeNode<T> Resolve(IList<string> nodeNames)
        {
            if (nodeNames.Count == 0)
                return null;

            if (nodeNames.First() != Root.Name)
                return null;

            IPathTreeNode<T> nodeVisitor = Root;
            IPathTreeNode<T> nextNode;

            foreach (var name in nodeNames.Skip(1))
            {
                if (!nodeVisitor.TryGetChild(name, out nextNode))
                {
                    return null;
                }
                nodeVisitor = nextNode;
            }

            return nodeVisitor;
        }

        /// <summary>
        /// Allows iteration over the PathTree
        /// </summary>
        /// <returns></returns>
        /// <remarks>Idea adapted from https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html 
        /// Implementation adapted from https://blogs.msdn.microsoft.com/wesdyer/2007/03/23/all-about-iterators/
        /// </remarks>
        public IEnumerable<IPathTreeNode<T>> EnumerateDepthFirst() => Root.SelfAndDescendantsDepthFirst();

        public IEnumerable<IPathTreeNode<T>> EnumerateBreadthFirst() => Root.SelfAndDescendantsBreadthFirst();

        //public IEnumerable<T> DescendantsDepthFirst(string nodePath = "")
        //{
        //    var nodeStack = new Stack<IPathTreeNode<T>>();

        //    foreach (var node in root.Children())
        //        nodeStack.Push(node);

        //    while (nodeStack.Count > 0)
        //    {
        //        var node = nodeStack.Pop();
        //        yield return node.Value;

        //        foreach (var child in node.Children())
        //            nodeStack.Push(child);
        //    }
        //}

        public int Count()
        {
            int nodeCount = 0;
            var nodeStack = new Stack<IPathTreeNode<T>>();

            nodeStack.Push(Root);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                nodeCount++;

                foreach (var child in node.Children())
                    nodeStack.Push(child);
            }

            return nodeCount;
        }
    }
}
