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
        public void Add_Items_AsExpected(IReadOnlyCollection<(string, int)> expectedItems)
        {
            var actualItems = new PathTree<int>();

            foreach(var item in expectedItems)
                actualItems.Add(item.Item1, item.Item2);

            PathTreeAssert.EqualsAll(actualItems, expectedItems);
        }

        [TestCase("/Folder/UnparentedItem", 5)]
        public void Add_UnparentedItem_ThrowsKeyNotFoundException(string name, int value)
        {
            var trie = new PathTree<int>();

            Assert.Throws<KeyNotFoundException>(() => trie.Add(name, value));
        }

        [TestCaseSource(typeof(PathTreeTestCases), "AddDuplicateCases")]
        public void Add_DuplicateItem_ThrowsArgumentException(IReadOnlyCollection<(string, int)> list,
            (string, int) duplicates)
        {
            var trie = new PathTree<int>();

            foreach(var item in list)
                trie.Add(item.Item1, item.Item2);

            Assert.Throws<ArgumentException>(() => trie.Add(duplicates.Item1, duplicates.Item2));
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
            //var trie = new PathTrie<int>();

            //foreach (var item in items)
            //    trie.Add(item.Item1, item.Item2);

            trie.TryGetNode(nodeKey, out var node);
            var actual = node.PathKey;

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(PathTreeTestCases), "EnumerateDepthFirstCases")]
        public void EnumerateDepthFirst_ReturnsExpected(IReadOnlyCollection<(string, int)> items,
            IReadOnlyCollection<IReadOnlyCollection<(string, int)>> expectedOrders)
        {
            var trie = new PathTree<int>();

            foreach (var item in items)
                trie.Add(item.Item1, item.Item2);

            var trieDescendants = trie.EnumerateDepthFirst().Select(x => (x.PathKey, x.Value)).ToList();
            var listItems = items.ToList();

            ListAssert.EqualsAny(trieDescendants, expectedOrders);
        }
    }
}
