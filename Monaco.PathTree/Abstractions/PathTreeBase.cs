using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monaco.PathTree.Abstractions
{
    public abstract class PathTreeBase<TNode, TItem> : IPathTree<TNode, TItem>
        where TNode : IPathNode<TNode, TItem>
    {
        private TNode _root;
        public TNode Root
        {
            get => _root;
            set
            {
                _root = value;
                if (_root is object)
                    _root.Parent = default;
            }
        }

        public bool ExcludeRootFromPath { get; set; }
        public string[] PathSeparators { get; set; } = new string[] { "/", "\\" };

        public PathTreeBase(TNode root)
        {
            if (root is null)
                ThrowHelper.ThrowArgumentNull(nameof(root));

            _root = root;
        }

        /// <summary>
        /// Attaches the node as the specified path if the parent exists and renames the node if necessary
        /// </summary>
        /// <param name="path">The full path associated with the node</param>
        /// <param name="node">The node to be attached</param>
        public virtual void AttachNodeAsPath(string path, TNode node)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var nodeNames = SplitPath(path);
            var parent = Resolve(nodeNames.Take(nodeNames.Count - 1).ToList());
            var nodeName = nodeNames.Last();

            if (parent is null)
                ThrowHelper.ThrowNodeNotFound(path);

            if (parent.ContainsChildNode(nodeName))
                ThrowHelper.ThrowNodeAlreadyExists(nodeName);

            node.Detach();
            node.Rename(nodeName);
            parent.AttachChildNode(node);
        }

        /// <summary>
        /// Attaches the node as a child of the specified path
        /// </summary>
        /// <param name="path">The full path associated with the parent</param>
        /// <param name="nodeName">Name of the node to add</param>
        /// <param name="item">The item to be added</param>
        /// <param name="metadata">Metadata to associate with the new node</param>
        public virtual void AttachNodeToPath(string path, TNode node)
        {
            if (path is null)
                ThrowHelper.ThrowArgumentNull(nameof(path));
            else if (ExcludeRootFromPath is false && string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            if (node is null)
                ThrowHelper.ThrowArgumentNull(nameof(node));

            var parent = ResolveNode(path);

            if (parent is null)
                ThrowHelper.ThrowNodeNotFound(path);

            node.Detach();
            parent.AttachChildNode(node);
        }

        /// <summary>
        /// Tries to get an existing item stored at the specified path
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="item"></param>
        /// <returns>True if successful, false if failed</returns>
        public virtual bool TryGetItem(string path, out TItem? item)
        {
            if (path is null)
                ThrowHelper.ThrowArgumentNull(nameof(path));
            else if (ExcludeRootFromPath is false && string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var node = ResolveNode(path);

            if (node is object)
            {
                item = node.Item;
                return true;
            }
            else
            {
                item = default;
                return false;
            }
        }

        /// <summary>
        /// Tries to get an existing item of a specific type stored at the specified path
        /// </summary>
        /// <typeparam name="TDerivedItem">Type of the item</typeparam>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="item"></param>
        /// <returns>True if successful, false if failed</returns>
        public virtual bool TryGetItem<TDerivedItem>(string path, out TDerivedItem? item) where TDerivedItem : TItem
        {
            if (path is null)
                ThrowHelper.ThrowArgumentNull(nameof(path));
            else if (ExcludeRootFromPath is false && string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var node = ResolveNode(path);

            if (node is object)
            {
                item = (TDerivedItem) node.Item!;
                return true;
            }
            else
            {
                item = default;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the node contained at the specified location
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="node"></param>
        /// <returns>True if found, false if not found</returns>
        public virtual bool TryGetNode(string path, out TNode? node)
        {
            if (path is null)
                ThrowHelper.ThrowArgumentNull(nameof(path));
            else if (ExcludeRootFromPath is false && string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            node = ResolveNode(path);

            return node is object;
        }

        /// <summary>
        /// Tries to get the node contained at the specified location
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        /// <param name="node"></param>
        /// <returns>True if found and of type TDerivedNode, false if not found or not of type TDerivedNode</returns>
        public virtual bool TryGetNode<TDerivedNode>(string path, out TDerivedNode? node)
            where TDerivedNode : TNode
        {
            if (path is null)
                ThrowHelper.ThrowArgumentNull(nameof(path));
            else if (ExcludeRootFromPath is false && string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var baseNode = ResolveNode(path);

            if (baseNode is TDerivedNode derivedNode)
            {
                node = derivedNode;
                return true;
            }
            else
            {
                node = default;
                return false;
            }
        }

        /// <summary>
        /// Removes the node at the specified location
        /// </summary>
        /// <param name="path">The full path associated with the item</param>
        public virtual void RemoveNode(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            var removeNode = ResolveNode(path);

            if (removeNode is null)
                ThrowHelper.ThrowNodeNotFound(path);

            if (removeNode.Parent is null)
                ThrowHelper.ThrowParentNodeNotFound(path);

            removeNode.Parent.RemoveChildNode(removeNode.Name);
        }
        
        public IEnumerable<string> CreatePaths(TNode node)
        {
            int skip = ExcludeRootFromPath is true ? 1 : 0;
            return node.SelfAndAncestors<TNode, TItem>().Select(x => x.Name).Reverse().Skip(skip);
        }

        public string CreatePathKey(TNode node) => CreatePathKey(node, PathSeparators[0]);

        public string CreatePathKey(TNode node, string separator)
        {
            var sb = new StringBuilder();
            foreach (var path in CreatePaths(node))
            {
                sb.Append(separator);
                sb.Append(path);
            }
            return sb.ToString();
        }

        protected virtual IList<string> SplitPath(string absolutePath) =>
            absolutePath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);

        protected virtual TNode? ResolveNode(string absolutePath)
        {
            var nodeNames = SplitPath(absolutePath);
            return Resolve(nodeNames);
        }

        protected virtual TNode? ResolveParent(string absolutePath)
        {
            var nodeNames = SplitPath(absolutePath);
            var parentNodeNames = nodeNames.Take(nodeNames.Count - 1).ToList();

            return Resolve(parentNodeNames);
        }

        protected virtual TNode? Resolve(IList<string> nodeNames)
        {
            if (nodeNames.Count == 0)
                return ExcludeRootFromPath is true ? Root : default;

            int skip = ExcludeRootFromPath is false ? 1 : 0;

            if (ExcludeRootFromPath == false && nodeNames.First() != Root.Name)
                return default;

            var nodeVisitor = Root;

            foreach (var name in nodeNames.Skip(skip))
            {
                if (!nodeVisitor.TryGetChildNode(name, out TNode? nextNode))
                {
                    return default;
                }
                nodeVisitor = nextNode!;
            }

            return nodeVisitor;
        }
    }
}
