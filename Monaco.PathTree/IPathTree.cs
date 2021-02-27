namespace Monaco.PathTree
{
    public interface IPathTree<TItem, TMetadata>
    {
        PathTreeNode<TItem, TMetadata> Root { get; }

        PathTreeNode<TItem, TMetadata> AddItemAsPath(string path, TItem item, TMetadata metadata);
        PathTreeNode<TItem, TMetadata> AddItemToPath(string path, string nodeName, TItem item, TMetadata metadata);

        bool TryGetItem(string path, out TItem item);
        bool TryGetItem<U>(string path, out U item) where U : TItem;
        bool TryGetMetadata(string path, out TMetadata metadata);
        bool TryGetNode(string path, out PathTreeNode<TItem, TMetadata> node);

        void RemoveNode(string path);

        int Count();
    }
}
