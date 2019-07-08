using System;
using System.Collections.Generic;
using System.Linq;
using Monaco.PathTree;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests.AssertHelpers
{
    static class PathTreeAssert
    {
        public static void EqualsAll<T>(PathTree<T> trie, IReadOnlyCollection<(string, T)> list) where T : IComparable<T>
        {
            var trieCount = trie.Count();

            if (trieCount != list.Count)
                Assert.Fail($"Collection size mismatch between {trieCount} and {list.Count}");
            else
            {
                var comparer = EqualityComparer<T>.Default;

                foreach (var item in list)
                {
                    if (trie.TryGetValue(item.Item1, out var trieItem))
                    {
                        if (!comparer.Equals(item.Item2, trieItem))
                            Assert.Fail($"List item {item.Item1} did not match {trieItem} in the {nameof(PathTree<T>)}");
                    }
                    else
                        Assert.Fail($"Key {item.Item1} was not found in the {nameof(PathTree<T>)}");
                }
            }
        }
    }
}
