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
        private IPathTreeNode<int> parent;
        private readonly (string, int)[] nodeChildren = new (string, int)[]
        {
            ("SubItem1", 1), ("SubItem2", 2), ("SubItem3", 3)
        };

        [SetUp]
        public void Setup()
        {
            parent = new PathTreeNode<int>("parent", -1);
            foreach (var item in nodeChildren)
                parent.AddChild(item.Item1, item.Item2);
        }

        [TestCase("TestItem1", 15)]
        public void AddChild_AsExpected(string name, int value)
        {
            parent.AddChild(name, value);

            parent.TryGetChild(name, out var node);

            Assert.Multiple(() =>
            {
                Assert.NotNull(node);
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(value, node.Value);
            });
        }

        [Test]
        public void AttachChild_AsExpected()
        {
            var expected = ("TestItem5", 5);
            parent.AttachChild(new PathTreeNode<int>(expected.Item1, expected.Item2));

            parent.TryGetChild(expected.Item1, out var node);

            Assert.Multiple(() =>
            {
                Assert.NotNull(node);
                Assert.AreEqual(expected.Item1, node.Name);
                Assert.AreEqual(expected.Item2, node.Value);
            });
        }

        [TestCase("SubItem3", 3)]
        public void DetachChild_AsExpected(string name, int value)
        {
            var node = parent.DetachChild(name);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(value, node.Value);
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
        public void TryGetChild_ReturnsExpected(string name, int expectedValue)
        {
            var isFound = parent.TryGetChild(name, out var node);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(isFound);
                Assert.NotNull(node);
                Assert.AreEqual(name, node.Name);
                Assert.AreEqual(expectedValue, node.Value);
            });
        }

        [Test]
        public void Children_ReturnsExpected()
        {
            var actual = parent.Children().Select(x => (x.Name, x.Value)).ToList();
            ListAssert.ContainsSameItems(nodeChildren, actual);
        }

        [TestCase("SubItem2", "SubItem5")]
        public void RenameChild_ReturnsExpected(string childName, string newName)
        {
            parent.RenameChild(childName, newName);

            Assert.Multiple(() =>
            {
                Assert.That(parent.TryGetChild(newName, out var actualChild));
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
            parent.TryGetChild(childName, out var child);
            child.Rename(newName);

            Assert.Multiple(() =>
            {
                Assert.That(parent.TryGetChild(newName, out var actualChild));
                Assert.AreEqual(newName, actualChild.Name);
            });
        }
    }
}
