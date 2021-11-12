using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests.AssertHelpers;

static class PathTreeAssert
{
    public static void EqualsAll<T>(PathTree<PathNode<T>, T> tree, IList<(string, T)> list) where T : IComparable<T>
    {
        var treeCount = tree.Count();

        if (treeCount != list.Count)
            Assert.Fail($"Collection size mismatch between {treeCount} and {list.Count}");
        else
        {
            var comparer = EqualityComparer<T>.Default;

            foreach (var item in list)
            {
                if (tree.TryGetItem(item.Item1, out var treeItem))
                {
                    if (!comparer.Equals(item.Item2, treeItem))
                        Assert.Fail($"List item {item.Item1} did not match {treeItem} in the {nameof(PathTree)}");
                }
                else
                    Assert.Fail($"Key {item.Item1} was not found in the {nameof(PathTree)}");
            }
        }
    }
}
