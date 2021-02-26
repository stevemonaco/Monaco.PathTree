using System.Linq;
using System.Collections.Generic;
using Monaco.PathTree;
using Monaco.PathTree.UnitTests.AssertHelpers;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests
{
    [TestFixture]
    class PathTreeNodeTests
    {
        private PathTreeNode<int, EmptyMetadata> parent;
        private readonly (string, int)[] nodeChildren = new (string, int)[]
        {
            ("SubItem1", 1), ("SubItem2", 2), ("SubItem3", 3)
        };

        [SetUp]
        public void Setup()
        {
            parent = new PathTreeNode<int, EmptyMetadata>("parent", -1);
            foreach (var item in nodeChildren)
                parent.AddChild(item.Item1, item.Item2);
        }

        [TestCase("TestItem1", 15)]
        public void AddChild_AsExpected(string name, int item)
        {
            parent.AddChild(name, item);

            parent.TryGetChildNode(name, out var node);

            Assert.Multiple(() =>
            {
                Assert.NotNull(node);
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(item, node.Item);
            });
        }

        [Test]
        public void AttachChild_AsExpected()
        {
            var expected = ("TestItem5", 5);
            parent.AttachChild(new PathTreeNode<int, EmptyMetadata>(expected.Item1, expected.Item2));

            parent.TryGetChildNode(expected.Item1, out var node);

            Assert.Multiple(() =>
            {
                Assert.NotNull(node);
                Assert.AreEqual(expected.Item1, node.Name);
                Assert.AreEqual(expected.Item2, node.Item);
            });
        }

        [TestCase("SubItem3", 3)]
        public void DetachChild_AsExpected(string name, int expectedItem)
        {
            var node = parent.DetachChild(name);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(expectedItem, node.Item);
            });
        }

        [TestCase("SubItem2")]
        public void RemoveChild_AsExpected(string name)
        {
            parent.RemoveChild(name);
            Assert.IsFalse(parent.ContainsChild(name));
        }

        [TestCase("SubItem1")]
        [TestCase("SubItem2")]
        [TestCase("SubItem3")]
        public void ContainsChild_Found_AsExpected(string name)
        {
            Assert.IsTrue(parent.ContainsChild(name));
        }

        [TestCase("SubItem18")]
        [TestCase("subitem1")]
        public void ContainsChild_NotFound_AsExpected(string name)
        {
            Assert.IsFalse(parent.ContainsChild(name));
        }

        [TestCase("SubItem2", 2)]
        public void TryGetChild_ReturnsExpected(string name, int expectedItem)
        {
            var isFound = parent.TryGetChildNode(name, out var node);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(isFound);
                Assert.NotNull(node);
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(expectedItem, node.Item);
            });
        }

        [Test]
        public void ChildNodes_ReturnsExpected()
        {
            var actual = parent.ChildNodes.Select(x => (x.Name, x.Item)).ToList();
            ListAssert.ContainsSameItems(nodeChildren, actual);
        }

        [TestCase("SubItem2", "SubItem5")]
        public void RenameChild_ReturnsExpected(string childName, string newName)
        {
            parent.RenameChild(childName, newName);

            Assert.Multiple(() =>
            {
                Assert.That(parent.TryGetChildNode(newName, out var actualChild));
                Assert.AreEqual(newName, actualChild.Name);
            });
        }

        [TestCase("newParentName")]
        public void Rename_Root_ReturnsExpected(string newName)
        {
            parent.Rename(newName);

            Assert.AreEqual(newName, parent.Name);
        }

        [TestCase("SubItem2", "NewSubItem2Name")]
        public void Rename_Child_ReturnsExpected(string childName, string newName)
        {
            parent.TryGetChildNode(childName, out var child);
            child.Rename(newName);

            Assert.Multiple(() =>
            {
                Assert.That(parent.TryGetChildNode(newName, out var actualChild));
                Assert.AreEqual(newName, actualChild.Name);
            });
        }
    }
}
