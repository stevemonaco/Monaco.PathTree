using System;
using System.Linq;
using System.Collections.Generic;

namespace Monaco.PathTree
{
    public class PathTreeNode<TItem, TMetadata>
    {
        protected IDictionary<string, PathTreeNode<TItem, TMetadata>> _children;

        public PathTreeNode<TItem, TMetadata> Parent { get; set; }
        public TItem Item { get; set; }
        public TMetadata Metadata { get; set; }
        public string Name { get; private set; }

        public IEnumerable<PathTreeNode<TItem, TMetadata>> ChildNodes => _children?.Values ?? Enumerable.Empty<PathTreeNode<TItem, TMetadata>>();
        public IEnumerable<TItem> ChildItems => _children?.Values.Select(x => x.Item) ?? Enumerable.Empty<TItem>();

        public PathTreeNode(string rootNodeName, TItem item, TMetadata metadata = default)
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
        public string PathKey => "/" + string.Join("/", this.SelfAndAncestors().Select(x => x.Name).Reverse());

        public IEnumerable<string> Paths => this.SelfAndAncestors().Select(x => x.Name).Reverse();

        /// <summary>
        /// Adds a child node created from parameters
        /// </summary>
        /// <param name="nodeName">Name of the node to add</param>
        /// <param name="item">Item associated with node</param>
        /// <param name="metadata">Metadata associated with node</param>
        /// <returns>The node which was added</returns>
        public PathTreeNode<TItem, TMetadata> AddChild(string nodeName, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(nodeName));

            if (_children is null)
                _children = new Dictionary<string, PathTreeNode<TItem, TMetadata>>();

            if (_children.ContainsKey(nodeName))
                ThrowHelper.ThrowNodeAlreadyExists(nodeName);

            var node = new PathTreeNode<TItem, TMetadata>(nodeName, item, metadata);
            node.Parent = this;
            _children.Add(nodeName, node);

            return node;
        }

        /// <summary>
        /// Attaches an existing node as a child
        /// </summary>
        /// <param name="node">Node to attach</param>
        public void AttachChildNode(PathTreeNode<TItem, TMetadata> node)
        {
            if (node is null)
                ThrowHelper.ThrowArgumentNull(nameof(node));

            if (_children is null)
                _children = new Dictionary<string, PathTreeNode<TItem, TMetadata>>();

            if (_children.ContainsKey(node.Name))
                ThrowHelper.ThrowNodeAlreadyExists(node.Name);

            node.Parent = this;
            _children.Add(node.Name, node);
        }

        /// <summary>
        /// Detaches a child node by name
        /// </summary>
        /// <param name="nodeName">Name of the node to detach</param>
        /// <returns></returns>
        public PathTreeNode<TItem, TMetadata> DetachChildNode(string nodeName)
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
                parent.AttachChildNode(this);
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
        public bool TryGetChildNode(string name, out PathTreeNode<TItem, TMetadata> node)
        {
            if(string.IsNullOrWhiteSpace(name))
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
