namespace Monaco.PathTree.UnitTests
{
    public static class TestTreeBuilder
    {
        public static PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata> BuildRootOnlyTree()
        {
            var root = new PathNode<int, EmptyMetadata>("Root", -1);
            return new PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata>(root);
        }

        public static PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata> BuildMultiLayerTree()
        {
            (string, int)[] testTreeChildren = new (string, int)[]
            {
                ("/Root/Folder1", 1), ("/Root/Folder1/Item1", 15), ("/Root/Folder1/Item2", 25),
                ("/Root/Folder2", 2), ("/Root/Folder2/Folder3", 3), ("/Root/Folder2/Folder3/Item3", 5)
            };

            var root = new PathNode<int, EmptyMetadata>("Root", -1);
            var tree = new PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata>(root);

            foreach (var item in testTreeChildren)
                tree.AddItemAsPath(item.Item1, item.Item2);

            return tree;
        }
    }
}
