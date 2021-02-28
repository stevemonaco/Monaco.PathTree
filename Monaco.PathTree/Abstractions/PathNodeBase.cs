using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monaco.PathTree.Abstractions
{
    public abstract class PathNodeBase<TNode, TItem, TMetadata> : IPathNode<TNode, TItem, TMetadata>
        where TNode : PathNodeBase<TNode, TItem, TMetadata>
    {
        protected IDictionary<string, TNode> _children;

        public TNode Parent { get; set; }
        public TItem Item { get; set; }
        public TMetadata Metadata { get; set; }
        public string Name { get; private set; }

        public IEnumerable<TNode> ChildNodes => _children?.Values ?? Enumerable.Empty<TNode>();
        public IEnumerable<TItem> ChildItems => _children?.Values.Select(x => x.Item) ?? Enumerable.Empty<TItem>();

        public PathNodeBase(string rootNodeName, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(rootNodeName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(rootNodeName));

            Name = rootNodeName;
            Item = item;
            Metadata = metadata;
        }

        /// <summary>
        /// Returns the path key
        /// </summary>
        /// <returns></returns>
        public string PathKey => "/" + string.Join("/", ((TNode)this).SelfAndAncestors<TNode, TItem, TMetadata>().Select(x => x.Name).Reverse());

        public IEnumerable<string> Paths => ((TNode)this).SelfAndAncestors<TNode, TItem, TMetadata>().Select(x => x.Name).Reverse();

        protected abstract TNode CreateNode(string nodeName, TItem item, TMetadata metadata = default);

        /// <summary>
        /// Adds a child node created from parameters
        /// </summary>
        /// <param name="nodeName">Name of the node to add</param>
        /// <param name="item">Item associated with node</param>
        /// <param name="metadata">Metadata associated with node</param>
        /// <returns>The node which was added</returns>
        public TNode AddChild(string nodeName, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(nodeName));

            if (_children is null)
                _children = new Dictionary<string, TNode>();

            if (_children.ContainsKey(nodeName))
                ThrowHelper.ThrowNodeAlreadyExists(nodeName);

            var node = CreateNode(nodeName, item, metadata);
            node.Parent = (TNode) this;
            _children.Add(nodeName, node);

            return node;
        }

        /// <summary>
        /// Attaches an existing node as a child
        /// </summary>
        /// <param name="node">Node to attach</param>
        public void AttachChildNode(TNode node)
        {
            if (node is null)
                ThrowHelper.ThrowArgumentNull(nameof(node));

            if (_children is null)
                _children = new Dictionary<string, TNode>();

            if (_children.ContainsKey(node.Name))
                ThrowHelper.ThrowNodeAlreadyExists(node.Name);

            node.Parent = (TNode) this;
            _children.Add(node.Name, node);
        }

        /// <summary>
        /// Detaches a child node by name
        /// </summary>
        /// <param name="nodeName">Name of the node to detach</param>
        /// <returns></returns>
        public TNode DetachChildNode(string nodeName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(nodeName));

            if (_children is null)
                ThrowHelper.ThrowNodeNotFound(nodeName);

            if (_children.TryGetValue(nodeName, out var node))
            {
                node.Parent = null;
                _children.Remove(nodeName);
                return node;
            }
            else
            {
                ThrowHelper.ThrowNodeNotFound(nodeName);
                return null;
            }
        }

        /// <summary>
        /// Removes a child node by name
        /// </summary>
        /// <param name="nodeName">Name of the node to remove</param>
        public void RemoveChildNode(string nodeName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(nodeName));

            if (_children is null)
                ThrowHelper.ThrowNodeNotFound(nodeName);

            if (_children.ContainsKey(nodeName))
                _children.Remove(nodeName);
            else
                ThrowHelper.ThrowNodeNotFound(nodeName);
        }

        /// <summary>
        /// Renames a child node
        /// </summary>
        /// <param name="name">Name of the existing child node</param>
        /// <param name="newName">New name</param>
        public void RenameChild(string name, string newName)
        {
            if (string.IsNullOrWhiteSpace(name))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(name));

            if (string.IsNullOrWhiteSpace(newName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(newName));

            if (_children is null)
                ThrowHelper.ThrowNodeNotFound(name);

            if (_children.TryGetValue(name, out var node))
            {
                node.Rename(newName);
            }
            else
                ThrowHelper.ThrowNodeNotFound(name);
        }

        /// <summary>
        /// Renames this node
        /// </summary>
        /// <param name="name">New name</param>
        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(name));

            var parent = Parent;

            if (parent is null)
            {
                Name = name;
            }
            else
            {
                parent.DetachChildNode(Name);
                Name = name;
                parent.AttachChildNode((TNode) this);
            }
        }

        /// <summary>
        /// Determines if this node contains a child with the specified name
        /// </summary>
        /// <param name="name">Name of child node</param>
        /// <returns>True if contained, false if not contained</returns>
        public bool ContainsChildNode(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(name));

            if (_children is null)
                return false;

            return _children.ContainsKey(name);
        }

        /// <summary>
        /// Tries to get a child node by name
        /// </summary>
        /// <param name="name">Name of child node to get</param>
        /// <param name="node"></param>
        /// <returns>True if found, false if not found</returns>
        public bool TryGetChildNode(string name, out TNode node)
        {
            if (string.IsNullOrWhiteSpace(name))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(name));

            node = default;

            if (_children is null)
                return false;

            if (_children.TryGetValue(name, out node))
                return true;

            return false;
        }
    }
}
