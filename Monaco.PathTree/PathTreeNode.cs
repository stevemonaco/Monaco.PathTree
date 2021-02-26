using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

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

        public PathTreeNode(string name, TItem item)
        {
            Name = name;
            Item = item;
        }

        public PathTreeNode(string name, TItem item, TMetadata metadata)
        {
            Name = name;
            Item = item;
            Metadata = metadata;
        }

        /// <summary>
        /// Returns the path key
        /// </summary>
        /// <returns></returns>
        public string PathKey => "/" + string.Join("/", this.SelfAndAncestors().Select(x => x.Name).Reverse());

        public IEnumerable<string> Paths => this.SelfAndAncestors().Select(x => x.Name).Reverse();

        public void AddChild(string name, TItem item, TMetadata metadata = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(AddChild)}: parameter '{nameof(name)}' was null or empty");

            if (_children is null)
                _children = new Dictionary<string, PathTreeNode<TItem, TMetadata>>();

            if (_children.ContainsKey(name))
                throw new ArgumentException($"{nameof(AddChild)}: child element with {nameof(name)} '{name}' already exists");

            var node = new PathTreeNode<TItem, TMetadata>(name, item, metadata);
            node.Parent = this;
            _children.Add(name, node);
        }

        public void AttachChild(PathTreeNode<TItem, TMetadata> node)
        {
            if(node is null)
                throw new ArgumentException($"{nameof(AttachChild)}: parameter '{nameof(node)}' was null or empty");

            if (_children is null)
                _children = new Dictionary<string, PathTreeNode<TItem, TMetadata>>();

            if (_children.ContainsKey(node.Name))
                throw new ArgumentException($"{nameof(AttachChild)}: child element with {nameof(node.Name)} '{node.Name}' already exists");

            node.Parent = this;
            _children.Add(node.Name, node);
        }

        public PathTreeNode<TItem, TMetadata> DetachChild(string name)
        {
            if (name is null)
                throw new ArgumentException($"{nameof(DetachChild)}: parameter '{nameof(name)}' was null or empty");

            if (_children is null)
                throw new ArgumentException($"{nameof(DetachChild)}: child element with {nameof(name)} '{name}' does not exist");

            if (_children.TryGetValue(name, out var node))
            {
                node.Parent = null;
                _children.Remove(name);
                return node;
            }
            else
                throw new KeyNotFoundException($"{nameof(DetachChild)}: child element with {nameof(name)} '{name}' does not exist");
        }

        public void RemoveChild(string name)
        {
            if(name is null)
                throw new ArgumentException($"{nameof(RemoveChild)}: parameter '{nameof(name)}' was null or empty");

            if (_children is null)
                throw new KeyNotFoundException($"{nameof(RemoveChild)}: child element with {nameof(name)} '{name}' does not exist");

            if (_children.ContainsKey(name))
                _children.Remove(name);
            else
                throw new KeyNotFoundException($"{nameof(RemoveChild)}: child element with {nameof(name)} '{name}' does not exist");
        }

        public void RenameChild(string oldName, string newName)
        {
            if (oldName is null)
                throw new ArgumentNullException($"{nameof(RenameChild)}: parameter '{nameof(oldName)}' was null or empty");

            if (newName is null)
                throw new ArgumentNullException($"{nameof(RenameChild)}: parameter '{nameof(newName)}' was null or empty");

            if (_children is null)
                throw new KeyNotFoundException($"{nameof(RenameChild)}: child element with {nameof(oldName)} '{oldName}' does not exist");

            if (_children.TryGetValue(oldName, out var node))
            {
                node.Rename(newName);
            }
            else
                throw new KeyNotFoundException($"{nameof(RemoveChild)}: child element with {nameof(oldName)} '{oldName}' does not exist");
        }

        public void Rename(string name)
        {
            if (name is null)
                throw new ArgumentException($"{nameof(Rename)}: parameter '{nameof(name)}' was null or empty");

            var parent = Parent;

            if (parent is null)
            {
                Name = name;
            }
            else
            {
                parent.DetachChild(Name);
                Name = name;
                parent.AttachChild(this);
            }
        }

        public bool ContainsChild(string name)
        {
            if (name is null)
                throw new ArgumentException($"{nameof(ContainsChild)}: parameter '{nameof(name)}' was null or empty");

            if (_children is null)
                return false;

            return _children.ContainsKey(name);
        }

        public bool TryGetChildNode(string name, out PathTreeNode<TItem, TMetadata> node)
        {
            if(name is null)
                throw new ArgumentException($"{nameof(TryGetChildNode)}: parameter '{nameof(name)}' was null or empty");

            node = default;

            if (_children is null)
                return false;

            if (_children.TryGetValue(name, out node))
                return true;

            return false;
        }

        //public bool TryGetChildNode<TDerivedItem>(string name, out PathTreeNode<TDerivedItem, TMetadata> node) where TDerivedItem : TItem
        //{
        //    if (name is null)
        //        throw new ArgumentException($"{nameof(TryGetChildNode)}: parameter '{nameof(name)}' was null or empty");

        //    node = default;

        //    if (_children is null)
        //        return false;

        //    if (_children.TryGetValue(name, out var resultNode))
        //    {
        //        node = (PathTreeNode<TDerivedItem, TMetadata>) resultNode;
        //        return true;
        //    }

        //    return false;
        //}
    }
}
