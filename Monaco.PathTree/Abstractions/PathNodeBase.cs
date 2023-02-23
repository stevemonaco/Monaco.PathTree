using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Monaco.PathTree.Abstractions;

/// <summary>
/// Base node class for implementing Path Tree nodes
/// </summary>
/// <typeparam name="TNode">Self-referential node</typeparam>
/// <typeparam name="TItem">Type of Item stored within the node</typeparam>
public abstract class PathNodeBase<TNode, TItem> : IPathNode<TNode, TItem>
    where TNode : PathNodeBase<TNode, TItem>
{
    protected IDictionary<string, TNode>? _children;

    public TNode? Parent { get; set; }
    public TItem Item { get; set; }
    public string Name { get; private set; }

    public IEnumerable<TNode> ChildNodes => _children?.Values ?? Enumerable.Empty<TNode>();
    public IEnumerable<TItem> ChildItems => _children?.Values.Select(x => x.Item) ?? Enumerable.Empty<TItem>();

    public PathNodeBase(string name, TItem item)
    {
        if (string.IsNullOrWhiteSpace(name))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(name));

        Name = name;
        Item = item;
    }

    /// <inheritdoc/>
    public virtual void AttachChildNode(TNode node)
    {
        if (node is null)
            ThrowHelper.ThrowArgumentNull(nameof(node));

        if (_children is null)
            _children = new Dictionary<string, TNode>();

        if (_children.ContainsKey(node.Name))
            ThrowHelper.ThrowNodeAlreadyExists(node.Name);

        if (node.Parent is object)
            ThrowHelper.ThrowNodeIsAlreadyAttached(node.Name, node.Parent.Name);

        node.Parent = (TNode)this;
        _children.Add(node.Name, node);
    }

    /// <inheritdoc/>
    public virtual bool TryGetChildNode(string childName, [MaybeNullWhen(false)] out TNode node)
    {
        if (string.IsNullOrWhiteSpace(childName))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(childName));

        node = default;

        if (_children is null)
            return false;

        if (_children.TryGetValue(childName, out node))
            return true;

        return false;
    }

    /// <inheritdoc/>
    public virtual bool ContainsChildNode(string childName)
    {
        if (string.IsNullOrWhiteSpace(childName))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(childName));

        if (_children is null)
            return false;

        return _children.ContainsKey(childName);
    }

    /// <inheritdoc/>
    public virtual void Detach()
    {
        if (Parent is object)
            Parent.DetachChildNode(Name);
    }

    /// <inheritdoc/>
    public virtual TNode DetachChildNode(string childName)
    {
        if (string.IsNullOrWhiteSpace(childName))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(childName));

        if (_children is null)
            ThrowHelper.ThrowNodeNotFound(childName);

        if (_children.TryGetValue(childName, out var node))
        {
            node.Parent = default;
            _children.Remove(childName);
            return node;
        }
        else
        {
            ThrowHelper.ThrowNodeNotFound(childName);
            return default;
        }
    }

    /// <inheritdoc/>
    public virtual void RemoveChildNode(string childName)
    {
        if (string.IsNullOrWhiteSpace(childName))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(childName));

        if (_children is null)
            ThrowHelper.ThrowNodeNotFound(childName);

        if (_children.ContainsKey(childName))
            _children.Remove(childName);
        else
            ThrowHelper.ThrowNodeNotFound(childName);
    }

    /// <inheritdoc/>
    public virtual void Rename(string name)
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
            parent.AttachChildNode((TNode)this);
        }
    }

    /// <inheritdoc/>
    public virtual void RenameChild(string childName, string newName)
    {
        if (string.IsNullOrWhiteSpace(childName))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(childName));

        if (string.IsNullOrWhiteSpace(newName))
            ThrowHelper.ThrowStringNullEmptyOrWhiteSpace(nameof(newName));

        if (_children is null)
            ThrowHelper.ThrowNodeNotFound(childName);

        if (_children.TryGetValue(childName, out var node))
        {
            node.Rename(newName);
        }
        else
            ThrowHelper.ThrowNodeNotFound(childName);
    }
}
