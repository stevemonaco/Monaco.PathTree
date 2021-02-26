using System;
using System.Collections.Generic;
using System.Linq;

namespace Monaco.PathTree
{
    public class PathTree<TItem> : PathTree<TItem, EmptyMetadata>
    {
        public PathTree(PathTreeNode<TItem, EmptyMetadata> root) :
            base(root)
        {
        }

        public PathTree(string rootName, TItem rootItem) :
            base(rootName, rootItem, default)
        {
        }
    }

    public class PathTree<TItem, TMetadata> : IPathTree<TItem, TMetadata>
    {
        private PathTreeNode<TItem, TMetadata> _root;
        public PathTreeNode<TItem, TMetadata> Root
        {
            get => _root;
            set
            {
                _root = value;
                if (_root is object)
                    _root.Parent = null;
            }
        }

        public char[] PathSeparators { get; set; } = new char[] { '\\', '/' };

        public PathTree(PathTreeNode<TItem, TMetadata> root)
        {
            Root = root;
        }

        public PathTree(string rootName, TItem rootItem, TMetadata rootMetadata = default)
        {
            Root = new PathTreeNode<TItem, TMetadata>(rootName, rootItem, rootMetadata);
        }

        /// <summary>
        /// Adds the item as the specified path if the parent exists
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="item">The item to be added</param>
        public void AddItemAsPath(string path, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(AddItemAsPath)}: parameter '{nameof(path)}' was null or empty");

            var parent = ResolveParent(path);

            if (parent is null)
                throw new KeyNotFoundException($"{nameof(TryGetItem)}: could not find {nameof(path)} '{path}'");

            var nodeName = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries).Last();
            parent.AddChild(nodeName, item, metadata);
        }

        /// <summary>
        /// Tries to get the item stored at the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <returns>True if successful, false if failed</returns>
        public bool TryGetItem(string path, out TItem item)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetItem)}: parameter '{nameof(path)}' was null or empty");

            var node = ResolveNode(path);
            
            if (node is object)
            {
                item = node.Item;
                return true;
            }

            item = default(TItem);
            return false;
        }

        /// <summary>
        /// Tries to get the item of a specific type stored at the specified path
        /// </summary>
        /// <typeparam name="U">Type of the item</typeparam>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <returns>True if successful, false if failed</returns>
        public bool TryGetItem<U>(string path, out U item) where U : TItem
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetItem)}: parameter '{nameof(path)}' was null or empty");

            var node = ResolveNode(path);

            if (node is object)
            {
                item = (U)node.Item;
                return true;
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Tries to get the node contained at the specified location
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool TryGetNode(string path, out PathTreeNode<TItem, TMetadata> node)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(TryGetNode)}: parameter '{nameof(path)}' was null or empty");

            node = ResolveNode(path);

            return node is object;
        }

        //public bool TryGetNode<TDerived>(string path, out IPathTreeNode<TDerived> node) where TDerived : TValue
        //{
        //    if (string.IsNullOrWhiteSpace(path))
        //        throw new ArgumentException($"{nameof(TryGetNode)}: parameter '{nameof(path)}' was null or empty");

        //    var resolvedNode = ResolveNode(path);

        //    if (resolvedNode is object)
        //    {
        //        node = (IPathTreeNode<TDerived>) resolvedNode;
        //        return true;
        //    }

        //    node = default;
        //    return false;
        //}

        /// <summary>
        /// Removes the node at the specified location
        /// </summary>
        /// <param name="path"></param>
        public void RemoveNode(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(RemoveNode)}: parameter '{nameof(path)}' was null or empty");

            var removeNode = ResolveNode(path);

            if (removeNode is null)
                throw new KeyNotFoundException($"RemoveNode was unable to locate {nameof(path)}");

            if (removeNode.Parent is null)
                throw new NullReferenceException();

            removeNode.Parent.RemoveChild(removeNode.Name);
        }

        private PathTreeNode<TItem, TMetadata> ResolveNode(string absolutePath)
        {
            var nodeNames = absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            return Resolve(nodeNames);
        }

        private PathTreeNode<TItem, TMetadata> ResolveParent(string absolutePath)
        {
            var nodeNames = absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            var parentNodeNames = nodeNames.Take(nodeNames.Length - 1).ToList();

            return Resolve(parentNodeNames);
        }

        private PathTreeNode<TItem, TMetadata> Resolve(IList<string> nodeNames)
        {
            if (nodeNames.Count == 0)
                return default;

            if (nodeNames.First() != Root.Name)
                return default;

            var nodeVisitor = Root;

            foreach (var name in nodeNames.Skip(1))
            {
                if (!nodeVisitor.TryGetChildNode(name, out PathTreeNode<TItem, TMetadata> nextNode))
                {
                    return default;
                }
                nodeVisitor = nextNode;
            }

            return nodeVisitor;
        }

        /// <summary>
        /// Traverses the tree to count the number of items contained within
        /// </summary>
        /// <returns>Number of items</returns>
        public int Count()
        {
            int nodeCount = 0;
            var nodeStack = new Stack<PathTreeNode<TItem, TMetadata>>();

            nodeStack.Push(Root);

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
