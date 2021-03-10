namespace Monaco.PathTree.UnitTests
{
    public static class TestTreeBuilder
    {
        public static PathTree<PathNode<int>, int> BuildRootOnlyTree()
        {
            var root = new PathNode<int>("Root", -1);
            return new PathTree<PathNode<int>, int>(root);
        }

        public static PathTree<PathNode<int>, int> BuildMultiLayerTree()
        {
            (string path, string name, int value)[] testTreeChildren = new (string, string, int)[]
            {
                ("/Root/Folder1", "Folder1", 1), ("/Root/Folder1/Item1", "Item1", 15), ("/Root/Folder1/Item2", "Item2", 25),
                ("/Root/Folder2", "Folder2", 2), ("/Root/Folder2/Folder3", "Folder3", 3), ("/Root/Folder2/Folder3/Item3", "Item3", 5)
            };

            var root = new PathNode<int>("Root", -1);
            var tree = new PathTree<PathNode<int>, int>(root);

            foreach (var (path, name, value) in testTreeChildren)
            {
                var node = new PathNode<int>(name, value);
                tree.AttachNodeAsPath(path, node);
            }

            return tree;
        }
    }
}
