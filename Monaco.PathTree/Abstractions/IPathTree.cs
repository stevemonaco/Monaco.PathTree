using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public interface IPathTree<TNode, TItem>
        where TNode : IPathNode<TNode, TItem>
    {
        TNode Root { get; }
        bool ExcludeRootFromPath { get; set; }
        string[] PathSeparators { get; set; }

        void AttachNodeAsPath(string path, TNode node);
        void AttachNodeToPath(string path, TNode node);

        bool TryGetItem(string path, out TItem item);
        bool TryGetItem<U>(string path, out U item) where U : TItem;
        bool TryGetNode(string path, out TNode node);

        void RemoveNode(string path);

        string CreatePathKey(TNode node);
        string CreatePathKey(TNode node, string separator);
    }
}
