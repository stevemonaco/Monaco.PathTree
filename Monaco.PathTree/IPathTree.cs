using System.Collections.Generic;

namespace Monaco.PathTree
{
    public interface IPathTree<T>
    {
        IPathTreeNode<T> Root { get; set; }

        void AddAsPath(string path, T value);
        bool TryGetValue(string path, out T value);
        bool TryGetValue<U>(string path, out U value) where U : T;
        bool TryGetNode(string path, out IPathTreeNode<T> node);
        bool TryGetNode<U>(string path, out IPathTreeNode<U> node) where U : T;
        void RemoveNode(string path);
        int Count();
    }
}
