using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monaco.PathTree.UnitTests
{
    [TestFixture]
    public class PathTreeMemoryLeakTests
    {
        //[Test]
        //public void RemoveChild_FreesMemory()
        //{
        //    var tree = new PathTree<string>();

        //    tree.Add("Level1", "TopNode");
        //    tree.TryGetNode("Level1", out var topNode);

        //    var midNode = new PathTreeNode<string>("Level2", "MidNode");
        //    var midRef = new WeakReference(midNode);

        //    var leafNode = new PathTreeNode<string>("Level3", "LeafNode");
        //    var leafRef = new WeakReference(leafNode);

        //    midNode.AttachChild(leafNode);
        //    topNode.AttachChild(midNode);

        //    topNode.RemoveChild("Level2");

        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //    GC.Collect();

        //    Assert.Multiple(() =>
        //    {
        //        Assert.IsFalse(midRef.IsAlive);
        //        Assert.IsFalse(leafRef.IsAlive);
        //    });

        //}
    }
}
