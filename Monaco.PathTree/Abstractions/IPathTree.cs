using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public interface IPathTree<TNode, TItem, TMetadata>
        where TNode : IPathNode<TNode, TItem, TMetadata>
    {
        TNode Root { get; }

        TNode AddItemAsPath(string path, TItem item, TMetadata metadata);
        TNode AddItemToPath(string path, string nodeName, TItem item, TMetadata metadata);

        void AttachNodeAsPath(string path, TNode node);
        void AttachNodeToPath(string path, TNode node);

        bool TryGetItem(string path, out TItem item);
        bool TryGetItem<U>(string path, out U item) where U : TItem;
        bool TryGetMetadata(string path, out TMetadata metadata);
        bool TryGetNode(string path, out TNode node);

        void RemoveNode(string path);

        int Count();
    }
}
