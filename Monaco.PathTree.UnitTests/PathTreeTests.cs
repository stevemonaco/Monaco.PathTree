using System;
using System.Linq;
using System.Collections.Generic;
using Monaco.PathTree;
using Monaco.PathTree.UnitTests.AssertHelpers;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests
{
    [TestFixture]
    class PathTreeTests
    {
        [TestCaseSource(typeof(PathTreeTestCases), "AddCases")]
        public void Add_Items_AsExpected(IList<(string, int)> items)
        {
            var root = items.First();
            var actualItems = new PathTree<int>(root.Item1.Replace("/", ""), root.Item2);

            foreach(var item in items.Skip(1))
                actualItems.Add(item.Item1, item.Item2);

            PathTreeAssert.EqualsAll(actualItems, items);
        }

        [TestCase("/Root/Folder/UnparentedItem", 5)]
        public void Add_UnparentedItem_ThrowsKeyNotFoundException(string name, int value)
        {
            var trie = new PathTree<int>("Root", -1);

            Assert.Throws<KeyNotFoundException>(() => trie.Add(name, value));
        }

        [TestCaseSource(typeof(PathTreeTestCases), "AddDuplicateCases")]
        public void Add_DuplicateItem_ThrowsArgumentException(IList<(string, int)> items,
            (string, int) duplicates)
        {
            var root = items.First();
            var actualItems = new PathTree<int>(root.Item1.Replace("/", ""), root.Item2);

            foreach(var item in items.Skip(1))
                actualItems.Add(item.Item1, item.Item2);

            Assert.Throws<ArgumentException>(() => actualItems.Add(duplicates.Item1, duplicates.Item2));
        }

        [TestCaseSource(typeof(PathTreeTestCases), "TryGetValueCases")]
        public void TryGetValue_AsExpected(IPathTree<int> trie, string path, int expected)
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(trie.TryGetValue(path, out var actual));
                Assert.AreEqual(expected, actual);
            });
        }

        [TestCaseSource(typeof(PathTreeTestCases), "PathKeyCases")]
        public void PathKey_ReturnsExpected(PathTree<int> trie, string nodeKey, string expected)
        {
            trie.TryGetNode(nodeKey, out var node);
            var actual = node.PathKey;

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(PathTreeTestCases), "EnumerateDepthFirstCases")]
        public void EnumerateDepthFirst_ReturnsExpected(IList<(string, int)> items,
            IList<(string, int)> expectedOrder)
        {
            var trie = new PathTree<int>("Root", -1);

            foreach (var item in items)
                trie.Add(item.Item1, item.Item2);

            var trieDescendants = trie.EnumerateDepthFirst().Select(x => (x.PathKey, x.Value)).ToList();
            var listItems = items.ToList();

            CollectionAssert.AreEqual(trieDescendants, expectedOrder);
        }
    }
}
