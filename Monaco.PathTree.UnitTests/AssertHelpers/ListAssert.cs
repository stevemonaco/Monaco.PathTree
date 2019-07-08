using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests.AssertHelpers
{
    static class ListAssert
    {
        public static void ContainsSameItems<T>(IList<T> listA, IList<T> listB) where T : IComparable<T>
        {
            if (listA.Count != listB.Count)
                Assert.Fail($"List size mismatch between {listA.Count} and {listB.Count}");
            else
            {
                var sortedA = listA.OrderBy(x => x);
                var sortedB = listB.OrderBy(x => x);

                var zip = sortedA.Zip(sortedB, (a, b) => new Tuple<T, T>(a, b));
                var comparer = Comparer<T>.Default;

                foreach (var pair in zip)
                {
                    if (comparer.Compare(pair.Item1, pair.Item2) != 0)
                        Assert.Fail($"item {pair.Item1} did not match {pair.Item2}");
                }
            }
        }

        public static void EqualsAll<T>(IList<T> listA, IList<T> listB) where T : IComparable<T>
        {
            if (listA.Count != listB.Count)
                Assert.Fail($"List size mismatch between {listA.Count} and {listB.Count}");

            var comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < listA.Count; i++)
            {
                if (!comparer.Equals(listA[i], listB[i]))
                    Assert.Fail($"item {listA[i]} did not match {listB[i]}");
            }
        }

        public static void EqualsAny<T>(IList<T> actualList, IReadOnlyCollection<IReadOnlyCollection<T>> expectedLists)
            where T : IComparable<T>
        {
            if (!expectedLists.Any(x => actualList.SequenceEqual(x)))
                Assert.Fail($"{nameof(EqualsAny)} no supplied list matches the given {nameof(actualList)}");
        }
    }
}
