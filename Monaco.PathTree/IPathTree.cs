using System.Collections.Generic;

namespace Monaco.PathTree
{
    public interface IPathTree<T>
    {
        void Add(string path, T value);
        bool TryGetValue(string path, out T value);
        bool TryGetNode(string path, out IPathTreeNode<T> node);
        void RemoveNode(string path);
        int Count();

        IEnumerable<IPathTreeNode<T>> EnumerateDepthFirst();
        IEnumerable<IPathTreeNode<T>> EnumerateBreadthFirst();
    }
}
