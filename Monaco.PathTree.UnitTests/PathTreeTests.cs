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
        [TestCase("/Root/Folder1")]
        public void AttachNodeToPath_AsExpected(string addPath)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            var node = new PathNode<int>("TestNode5", 15);

            tree.AttachNodeToPath(addPath, node);

            var result = tree.TryGetNode(addPath, out _);

            Assert.IsTrue(result);
        }

        [Test]
        public void AttachNodeToPath_ExcludeRootFromPath_WithEmptyPath_AddsToRoot()
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            tree.ExcludeRootFromPath = true;

            var node = new PathNode<int>("TestNode", 15);

            tree.AttachNodeToPath("", node);

            if (tree.TryGetNode("/TestNode", out var actualNode))
                Assert.AreEqual(node.Name, actualNode.Name);
            else
                Assert.Fail($"Could not locate attached node");
        }

        [TestCase("/Root/Folder1")]
        public void AttachNodeToPath_NodeAlreadyExists_ThrowsArgumentException(string addPath)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            var node = new PathNode<int>("Item1", 15);

            Assert.Throws<ArgumentException>(() => 
                tree.AttachNodeToPath(addPath, node)
            );
        }

        [TestCase("/Root/Folder1/TestNode")]
        public void AttachNodeAsPath_AsExpected(string addPath)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            var node = new PathNode<int>("TestNode", 15);

            tree.AttachNodeAsPath(addPath, node);
            var result = tree.TryGetNode(addPath, out _);

            Assert.IsTrue(result);
        }

        [TestCase("/Root/Folder/UnparentedItem", 5)]
        public void AttachNodeAsPath_UnparentedItem_ThrowsKeyNotFoundException(string path, int value)
        {
            var root = new PathNode<int>("Root", -1);
            var tree = new PathTree<PathNode<int>, int>(root);
            var node = new PathNode<int>("UnparentedItem", value);

            Assert.Throws<KeyNotFoundException>(() => tree.AttachNodeAsPath(path, node));
        }

        [TestCase("/Folder1", "TestNode5", "/Folder1/TestNode5")]
        public void AttachNodeToPath_ExcludeRootFromPath_AsExpected(string addPath, string nodeName, string expectedPathKey)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            var node = new PathNode<int>(nodeName, 15);
            tree.ExcludeRootFromPath = true;

            tree.AttachNodeToPath(addPath, node);

            var result = tree.TryGetNode(expectedPathKey, out _);

            Assert.IsTrue(result);
        }

        [TestCase("/Root/Folder1/TestNode")]
        public void AttachNodeAsPath_RequiringNodeRename_Successful(string addPath)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            var node = new PathNode<int>("MismatchingName", 15);

            tree.AttachNodeAsPath(addPath, node);
            var result = tree.TryGetNode(addPath, out _);

            Assert.IsTrue(result);
        }

        [TestCase("/Root/Folder1/Item1")]
        [TestCase("/Root/Folder1/Item2")]
        public void AttachNodeAsPath_NodeAlreadyExists_ThrowsArgumentException(string addPath)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            var node = new PathNode<int>("Item1", 15);

            Assert.Throws<ArgumentException>(() =>
                tree.AttachNodeAsPath(addPath, node)
            );
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.TryGetItemCases))]
        public void TryGetItem_AsExpected(PathTree<PathNode<int>, int> tree, string path, int expected)
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(tree.TryGetItem(path, out var actual));
                Assert.AreEqual(expected, actual);
            });
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.RemoveNodeCases))]
        public void RemoveNode_AsExpected(PathTree<PathNode<int>, int> tree, string path)
        {
            tree.RemoveNode(path);
            Assert.IsFalse(tree.TryGetNode(path, out _));
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.PathKeyCases))]
        public void CreatePathKey_ReturnsExpected(PathTree<PathNode<int>, int> tree, string nodeKey, string expected)
        {
            tree.TryGetNode(nodeKey, out var node);
            var actual = tree.CreatePathKey(node);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("/Folder1/Item1")]
        [TestCase("/Folder1")]
        public void CreatePathKey_ExcludeRootFromPath_ReturnsExpected(string nodeKey)
        {
            var tree = TestTreeBuilder.BuildMultiLayerTree();
            tree.ExcludeRootFromPath = true;

            tree.TryGetNode(nodeKey, out var node);
            var actual = tree.CreatePathKey(node);

            Assert.AreEqual(nodeKey, actual);
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.CountCases))]
        public void Count_ReturnsExpected(PathTree<PathNode<int>, int> tree, int count)
        {
            Assert.AreEqual(count, tree.Count());
        }

        [TestCaseSource(typeof(PathTreeTestCases), nameof(PathTreeTestCases.EnumerateDepthFirstCases))]
        public void EnumerateDepthFirst_ReturnsExpected(IList<(string, int)> items,
            IList<(string, int)> expectedOrder)
        {
            var root = new PathNode<int>("Root", -1);
            var tree = new PathTree<PathNode<int>, int>(root);

            foreach (var item in items)
            {
                var name = item.Item1.Split("/").Last();
                var node = new PathNode<int>(name, item.Item2);
                tree.AttachNodeAsPath(item.Item1, node);
            }

            var treeDescendants = tree.EnumerateDepthFirst().Select(x => (tree.CreatePathKey(x), x.Item)).ToList();
            var listItems = items.ToList();

            CollectionAssert.AreEqual(treeDescendants, expectedOrder);
        }
    }
}
