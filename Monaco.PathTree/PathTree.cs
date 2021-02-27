using System;
using System.Collections.Generic;
using System.Linq;

namespace Monaco.PathTree
{
    public class PathTree<TItem> : PathTree<TItem, EmptyMetadata>
    {
        public PathTree(PathNode<TItem, EmptyMetadata> root) :
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
        private PathNode<TItem, TMetadata> _root;
        public PathNode<TItem, TMetadata> Root
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

        public PathTree(PathNode<TItem, TMetadata> root)
        {
            Root = root;
        }

        public PathTree(string rootName, TItem rootItem, TMetadata rootMetadata = default)
        {
            Root = new PathNode<TItem, TMetadata>(rootName, rootItem, rootMetadata);
        }

        /// <summary>
        /// Adds the item as the specified path if the parent exists
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="item">The item to be added</param>
        /// <param name="metadata">Metadata to associate with the path</param>
        public PathNode<TItem, TMetadata> AddItemAsPath(string path, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var parent = ResolveParent(path);

            if (parent is null)
                ThrowHelper.ThrowNodeNotFound(path);

            var nodeName = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries).Last();
            return parent.AddChild(nodeName, item, metadata);
        }

        /// <summary>
        /// Adds the item as a child of the specified path
        /// </summary>
        /// <param name="path">The full path associated with the parent</param>
        /// <param name="nodeName">Name of the node to add</param>
        /// <param name="item">The item to be added</param>
        /// <param name="metadata">Metadata to associate with the new node</param>
        public PathNode<TItem, TMetadata> AddItemToPath(string path, string nodeName, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            if (string.IsNullOrWhiteSpace(nodeName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(nodeName));

            var parent = ResolveNode(path);

            if (parent is null)
                ThrowHelper.ThrowNodeNotFound(path);

            return parent.AddChild(nodeName, item, metadata);
        }

        /// <summary>
        /// Tries to get an existing item stored at the specified path
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="item"></param>
        /// <returns>True if successful, false if failed</returns>
        public bool TryGetItem(string path, out TItem item)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var node = ResolveNode(path);
            
            if (node is object)
            {
                item = node.Item;
                return true;
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Tries to get an existing item of a specific type stored at the specified path
        /// </summary>
        /// <typeparam name="U">Type of the item</typeparam>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="item"></param>
        /// <returns>True if successful, false if failed</returns>
        public bool TryGetItem<U>(string path, out U item) where U : TItem
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

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
        /// Tries to get an existing metadata stored at the specified path
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public bool TryGetMetadata(string path, out TMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var node = ResolveNode(path);

            if (node is object)
            {
                metadata = node.Metadata;
                return true;
            }

            metadata = default;
            return false;
        }

        /// <summary>
        /// Tries to get the node contained at the specified location
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool TryGetNode(string path, out PathNode<TItem, TMetadata> node)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            node = ResolveNode(path);

            return node is object;
        }

        /// <summary>
        /// Removes the node at the specified location
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        public void RemoveNode(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var removeNode = ResolveNode(path);

            if (removeNode is null)
                ThrowHelper.ThrowNodeNotFound(path);

            if (removeNode.Parent is null)
                ThrowHelper.ThrowParentNodeNotFound(path);

            removeNode.Parent.RemoveChildNode(removeNode.Name);
        }

        private PathNode<TItem, TMetadata> ResolveNode(string absolutePath)
        {
            var nodeNames = absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            return Resolve(nodeNames);
        }

        private PathNode<TItem, TMetadata> ResolveParent(string absolutePath)
        {
            var nodeNames = absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            var parentNodeNames = nodeNames.Take(nodeNames.Length - 1).ToList();

            return Resolve(parentNodeNames);
        }

        private PathNode<TItem, TMetadata> Resolve(IList<string> nodeNames)
        {
            if (nodeNames.Count == 0)
                return default;

            if (nodeNames.First() != Root.Name)
                return default;

            var nodeVisitor = Root;

            foreach (var name in nodeNames.Skip(1))
            {
                if (!nodeVisitor.TryGetChildNode(name, out PathNode<TItem, TMetadata> nextNode))
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
            var nodeStack = new Stack<PathNode<TItem, TMetadata>>();

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
