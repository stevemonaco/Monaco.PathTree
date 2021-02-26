namespace Monaco.PathTree
{
    public interface IPathTree<TItem, TMetadata>
    {
        PathTreeNode<TItem, TMetadata> Root { get; set; }

        void AddItemAsPath(string path, TItem value, TMetadata metadata);
        bool TryGetItem(string path, out TItem value);
        bool TryGetItem<U>(string path, out U value) where U : TItem;
        bool TryGetNode(string path, out PathTreeNode<TItem, TMetadata> node);
        void RemoveNode(string path);
        int Count();
    }
}
