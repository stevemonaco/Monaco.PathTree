using System;
using System.Linq;
using System.Collections.Generic;
using Monaco.PathTree.UnitTests.AssertHelpers;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests
{
    [TestFixture]
    class PathTreeTests
    {
        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.AddItemAsPathCases))]
        public void AddItemAsPath_AsExpected(IList<(string, int)> items)
        {
            var root = items.First();
            var actualItems = new PathTree<int>(root.Item1.Replace("/", ""), root.Item2);

            foreach (var item in items.Skip(1))
                actualItems.AddItemAsPath(item.Item1, item.Item2);

            PathTreeAssert.EqualsAll(actualItems, items);
        }

        [TestCase("/Root/Folder/UnparentedItem", 5)]
        public void AddItemAsPath_UnparentedItem_ThrowsKeyNotFoundException(string name, int value)
        {
            var tree = new PathTree<int>("Root", -1);

            Assert.Throws<KeyNotFoundException>(() => tree.AddItemAsPath(name, value));
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.AddItemAsPathDuplicateCases))]
        public void AddItemAsPath_DuplicateNode_ThrowsArgumentException(IList<(string, int)> items,
            (string, int) duplicates)
        {
            var root = items.First();
            var actualItems = new PathTree<int>(root.Item1.Replace("/", ""), root.Item2);

            foreach (var item in items.Skip(1))
                actualItems.AddItemAsPath(item.Item1, item.Item2);

            Assert.Throws<ArgumentException>(() => actualItems.AddItemAsPath(duplicates.Item1, duplicates.Item2));
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.AddItemToPathCases))]
        public void AddItemToPath_AsExpected(PathTree<int> tree, string parentPath, string nodePath, string nodeName, int item)
        {
            var node = tree.AddItemToPath(parentPath, nodeName, item);

            if (tree.TryGetNode(nodePath, out var actualNode))
            {
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(node.Item, actualNode.Item);
                    Assert.AreEqual(node.PathKey, actualNode.PathKey);
                });
            }
            else
                Assert.Fail("Could not find node");
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.AddItemToPathDuplicateCases))]
        public void AddItemToPath_DuplicateNode_ThrowsArgumentException(PathTree<int> tree, string parentPath, string nodeName)
        {
            Assert.Throws<ArgumentException>(() => tree.AddItemToPath(parentPath, nodeName, 5));
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.TryGetItemCases))]
        public void TryGetItem_AsExpected(PathTree<int> tree, string path, int expected)
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(tree.TryGetItem(path, out var actual));
                Assert.AreEqual(expected, actual);
            });
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.RemoveNodeCases))]
        public void RemoveNode_AsExpected(PathTree<int> tree, string path)
        {
            tree.RemoveNode(path);
            Assert.IsFalse(tree.TryGetNode(path, out _));
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.PathKeyCases))]
        public void PathKey_ReturnsExpected(PathTree<int> tree, string nodeKey, string expected)
        {
            tree.TryGetNode(nodeKey, out var node);
            var actual = node.PathKey;

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.CountCases))]
        public void Count_ReturnsExpected(PathTree<int> tree, int count)
        {
            Assert.AreEqual(count, tree.Count());
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.EnumerateDepthFirstCases))]
        public void EnumerateDepthFirst_ReturnsExpected(IList<(string, int)> items,
            IList<(string, int)> expectedOrder)
        {
            var tree = new PathTree<int>("Root", -1);

            foreach (var item in items)
                tree.AddItemAsPath(item.Item1, item.Item2);

            var treeDescendants = tree.EnumerateDepthFirst().Select(x => (x.PathKey, x.Item)).ToList();
            var listItems = items.ToList();

            CollectionAssert.AreEqual(treeDescendants, expectedOrder);
        }
    }
}
