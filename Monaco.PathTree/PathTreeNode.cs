using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Monaco.PathTree
{
    public class PathTreeNode<T> : IPathTreeNode<T>, IEnumerable<IPathTreeNode<T>>
    {
        private IDictionary<string, IPathTreeNode<T>> children;

        public IPathTreeNode<T> Parent { get; set; }
        public T Value { get; set; }
        public string Name { get; private set; }

        public PathTreeNode(string name, T value)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Returns the path key
        /// </summary>
        /// <returns></returns>
        public string PathKey => "/" + string.Join("/", SelfAndAncestors().Select(x => x.Name).Reverse().ToList());

        public IEnumerable<string> Paths => SelfAndAncestors().Select(x => x.Name).Reverse();

        public void AddChild(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(AddChild)}: parameter '{nameof(name)}' was null or empty");

            if (children is null)
                children = new Dictionary<string, IPathTreeNode<T>>();

            if (children.ContainsKey(name))
                throw new ArgumentException($"{nameof(AddChild)}: child element with {nameof(name)} '{name}' already exists");

            var node = new PathTreeNode<T>(name, value);
            node.Parent = this;
            children.Add(name, node);
        }

        public void AttachChild(IPathTreeNode<T> node)
        {
            if(node is null)
                throw new ArgumentException($"{nameof(AttachChild)}: parameter '{nameof(node)}' was null or empty");

            if (children is null)
                children = new Dictionary<string, IPathTreeNode<T>>();

            if (children.ContainsKey(node.Name))
                throw new ArgumentException($"{nameof(AttachChild)}: child element with {nameof(node.Name)} '{node.Name}' already exists");

            node.Parent = this;
            children.Add(node.Name, node);
        }

        public IPathTreeNode<T> DetachChild(string name)
        {
            if (name is null)
                throw new ArgumentException($"{nameof(DetachChild)}: parameter '{nameof(name)}' was null or empty");

            if (children is null)
                throw new ArgumentException($"{nameof(DetachChild)}: child element with {nameof(name)} '{name}' does not exist");

            if (children.TryGetValue(name, out var node))
            {
                node.Parent = null;
                children.Remove(name);
                return node;
            }
            else
                throw new KeyNotFoundException($"{nameof(DetachChild)}: child element with {nameof(name)} '{name}' does not exist");
        }

        public void RemoveChild(string name)
        {
            if(name is null)
                throw new ArgumentException($"{nameof(RemoveChild)}: parameter '{nameof(name)}' was null or empty");

            if (children is null)
                throw new ArgumentException($"{nameof(RemoveChild)}: child element with {nameof(name)} '{name}' does not exist");

            if (children.ContainsKey(name))
                children.Remove(name);
            else
                throw new KeyNotFoundException($"{nameof(RemoveChild)}: child element with {nameof(name)} '{name}' does not exist");
        }

        public bool ContainsChild(string name)
        {
            if (name is null)
                throw new ArgumentException($"{nameof(ContainsChild)}: parameter '{nameof(name)}' was null or empty");

            if (children is null)
                return false;

            return children.ContainsKey(name);
        }

        public bool TryGetChild(string name, out IPathTreeNode<T> node)
        {
            if(name is null)
                throw new ArgumentException($"{nameof(TryGetChild)}: parameter '{nameof(name)}' was null or empty");

            node = default;

            if (children is null)
                return false;

            if (children.TryGetValue(name, out node))
                return true;

            return false;
        }

        public bool TryGetChild<U>(string name, out IPathTreeNode<U> node) where U : T
        {
            if (name is null)
                throw new ArgumentException($"{nameof(TryGetChild)}: parameter '{nameof(name)}' was null or empty");

            node = default;

            if (children is null)
                return false;

            if (children.TryGetValue(name, out var resultNode))
            {
                node = (IPathTreeNode<U>) resultNode;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ancestors of the specified node.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPathTreeNode<T>> Ancestors()
        {
            IPathTreeNode<T> nodeVisitor = Parent;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        public IEnumerable<IPathTreeNode<T>> SelfAndAncestors()
        {
            IPathTreeNode<T> nodeVisitor = this;

            while (nodeVisitor != null)
            {
                yield return nodeVisitor;
                nodeVisitor = nodeVisitor.Parent;
            }
        }

        public IEnumerator<IPathTreeNode<T>> GetEnumerator()
        {
            if (children is null)
                yield break;

            foreach (var child in children)
            {
                yield return child.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<IPathTreeNode<T>> SelfAndDescendantsDepthFirst()
        {
            var nodeStack = new Stack<IPathTreeNode<T>>();

            nodeStack.Push(this);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                yield return node;
                foreach (var child in node.Children().OrderByDescending(x => x.Name))
                    nodeStack.Push(child);
            }
        }

        public IEnumerable<IPathTreeNode<T>> SelfAndDescendantsBreadthFirst()
        {
            var nodeQueue = new Queue<IPathTreeNode<T>>();

            nodeQueue.Enqueue(this);

            while (nodeQueue.Count > 0)
            {
                var node = nodeQueue.Dequeue();
                yield return node;
                foreach (var child in node.Children().OrderByDescending(x => x.Name))
                    nodeQueue.Enqueue(child);
            }
        }

        public IEnumerable<IPathTreeNode<T>> DescendantsDepthFirst()
        {
            if (children is null)
                yield break;

            var nodeStack = new Stack<IPathTreeNode<T>>(children.Values);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                yield return node;
                foreach (var child in node.Children().OrderByDescending(x => x.Name))
                    nodeStack.Push(child);
            }
        }

        public IEnumerable<IPathTreeNode<T>> DescendantsBreadthFirst()
        {
            if (children is null)
                yield break;

            var nodeQueue = new Queue<IPathTreeNode<T>>();

            nodeQueue.Enqueue(this);

            while (nodeQueue.Count > 0)
            {
                var node = nodeQueue.Dequeue();
                yield return node;
                foreach (var child in node.Children().OrderByDescending(x => x.Name))
                    nodeQueue.Enqueue(child);
            }
        }

        public IEnumerable<IPathTreeNode<T>> Children()
        {
            if(children is null)
                yield break;

            foreach (var child in children.Values.OrderByDescending(x => x.Name))
                yield return child;
        }

    }
}
