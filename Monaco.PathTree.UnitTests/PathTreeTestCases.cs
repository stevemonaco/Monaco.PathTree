using System.Collections.Generic;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests
{
    public class PathTreeTestCases
    {
        private static PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata> BuildRootOnlyTree()
        {
            var root = new PathNode<int, EmptyMetadata>("Root", -1);
            return new PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata>(root);
        }

        private static PathTree<PathNode<int, EmptyMetadata>, int, EmptyMetadata> BuildMultiLayerTree()
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

        public static IEnumerable<TestCaseData> AddItemAsPathCases()
        {
            var rootItemList = new List<(string, int)> { ("/Root", -1) };
            yield return new TestCaseData(rootItemList);

            var singleItemList = new List<(string, int)> { ("/Root", -1), ("/Root/Folder1", 1) };
            yield return new TestCaseData(singleItemList);

            var nestedList = new List<(string, int)> {
                ("/Root", -1),
                ("/Root/Folder1", 1),
                ("/Root/Folder1/Item1", 15),
                ("/Root/Folder1/Item2", 25),
                ("/Root/Folder2", 2),
                ("/Root/Folder2/Folder3", 3),
                ("/Root/Folder2/Folder3/Item3", 5)
            };
            yield return new TestCaseData(nestedList);
        }

        public static IEnumerable<TestCaseData> AddItemAsPathDuplicateCases()
        {
            var items = new List<(string, int)> { ("/Root", -1), ("/Root/Folder1", 1), ("/Root/Folder1/Item1", 15) };
            var rootDuplicate = ("/Root/Folder1", 1);

            yield return new TestCaseData(items, rootDuplicate);

            var childDuplicate = ("/Root/Folder1/Item1", 15);

            yield return new TestCaseData(items, childDuplicate);
        }

        public static IEnumerable<TestCaseData> AddItemToPathCases()
        {
            var tree = BuildMultiLayerTree();
            yield return new TestCaseData(tree, "/Root/Folder1/Item1", "/Root/Folder1/Item1/TestNode", "TestNode", 8);

            tree = BuildMultiLayerTree();
            yield return new TestCaseData(tree, "/Root", "/Root/TestNode", "TestNode", 8);
        }

        public static IEnumerable<TestCaseData> AddItemToPathDuplicateCases()
        {
            var tree = BuildMultiLayerTree();
            yield return new TestCaseData(tree, "/Root/Folder1", "Item1");

            tree = BuildMultiLayerTree();
            yield return new TestCaseData(tree, "/Root", "Folder1");
        }

        public static IEnumerable<TestCaseData> EnumerateDepthFirstCases()
        {
            var rootOnlyList = new List<(string, int)> { };
            var rootItemExpected = new List<(string, int)> { ("/Root", -1) };
            yield return new TestCaseData(rootOnlyList, rootItemExpected);

            var singleItemList = new List<(string, int)> { ("/Root/Folder1", 1) };
            var singleItemExpected = new List<(string, int)> { ("/Root", -1), ("/Root/Folder1", 1) };
            yield return new TestCaseData(singleItemList, singleItemExpected);

            var nestedItemList = new List<(string, int)>
            {
                ("/Root/Folder1", 1),
                ("/Root/Folder1/Folder2", 2),
                ("/Root/Folder1/Folder2/Item3", 5),
            };
            var nestedItemExpected = new List<(string, int)>
            {
                ("/Root", -1),
                ("/Root/Folder1", 1),
                ("/Root/Folder1/Folder2", 2),
                ("/Root/Folder1/Folder2/Item3", 5),
            };
            yield return new TestCaseData(nestedItemList, nestedItemExpected);

            var manyItemList = new List<(string, int)>
            {
                ("/Root/Folder2", 22),
                ("/Root/Folder2/ItemB", 27),
                ("/Root/Folder2/ItemA", 17),
                ("/Root/Folder1", 11),
                ("/Root/Folder1/Folder3", 3),
                ("/Root/Folder1/Folder3/Item3", 35)
            };
            var manyItemExpected = new List<(string, int)>
            {
                ("/Root", -1),
                ("/Root/Folder1", 11),
                ("/Root/Folder1/Folder3", 3),
                ("/Root/Folder1/Folder3/Item3", 35),
                ("/Root/Folder2", 22),
                ("/Root/Folder2/ItemA", 17),
                ("/Root/Folder2/ItemB", 27)
            };
            yield return new TestCaseData(manyItemList, manyItemExpected);
        }

        public static IEnumerable<TestCaseData> PathKeyCases()
        {
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder1", "/Root/Folder1");
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder1/Item2/", "/Root/Folder1/Item2");
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder2/Folder3/Item3", "/Root/Folder2/Folder3/Item3");
        }

        public static IEnumerable<TestCaseData> TryGetItemCases()
        {
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root", -1);
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder2", 2);
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder2", 2);
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder2/Folder3/Item3", 5);
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder2/Folder3/Item3", 5);
        }

        public static IEnumerable<TestCaseData> RemoveNodeCases()
        {
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder1/Item2");
            yield return new TestCaseData(BuildMultiLayerTree(), "/Root/Folder1");
        }

        public static IEnumerable<TestCaseData> CountCases()
        {
            yield return new TestCaseData(BuildMultiLayerTree(), 7);
            yield return new TestCaseData(BuildRootOnlyTree(), 1);
        }
    }
}
