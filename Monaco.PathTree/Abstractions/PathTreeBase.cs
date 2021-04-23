using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monaco.PathTree.Abstractions
{
    /// <summary>
    /// Base tree class for a tree whose nodes can be accessed by paths
    /// </summary>
    /// <typeparam name="TNode">Type of node</typeparam>
    /// <typeparam name="TItem">Type of item stored within the node</typeparam>
    public abstract class PathTreeBase<TNode, TItem> : IPathTree<TNode, TItem>
        where TNode : IPathNode<TNode, TItem>
    {
        private TNode _root;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool ExcludeRootFromPath { get; set; }

        /// <inheritdoc/>
        public string[] PathSeparators { get; set; } = new string[] { "/", "\\" };

        public PathTreeBase(TNode root)
        {
            if (root is null)
                ThrowHelper.ThrowArgumentNull(nameof(root));

            _root = root;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public virtual bool TryGetNode(string path, out TNode? node)
        {
            if (path is null)
                ThrowHelper.ThrowArgumentNull(nameof(path));
            else if (ExcludeRootFromPath is false && string.IsNullOrWhiteSpace(path))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(path));

            node = ResolveNode(path);

            return node is object;
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Creates a sequence of strings which represent each level of the tree 
        /// </summary>
        /// <param name="node">Node to create paths from</param>
        /// <returns>The sequence of path levels</returns>
        public virtual IEnumerable<string> CreatePaths(TNode node)
        {
            int skip = ExcludeRootFromPath is true ? 1 : 0;
            return node.SelfAndAncestors<TNode, TItem>().Select(x => x.Name).Reverse().Skip(skip);
        }

        /// <inheritdoc/>
        public virtual string CreatePathKey(TNode node) => CreatePathKey(node, PathSeparators[0]);

        /// <inheritdoc/>
        public virtual string CreatePathKey(TNode node, string separator)
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
