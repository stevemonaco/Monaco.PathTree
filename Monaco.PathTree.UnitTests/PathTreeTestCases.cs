using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using NUnit.Framework;
using Monaco.PathTree;
using System.Linq;

namespace Monaco.PathTree.UnitTests
{
    public class PathTreeTestCases
    {
        private static IPathTree<int> BuildTestTree()
        {
            (string, int)[] testTreeChildren = new (string, int)[]
            {
                ("/Root/Folder1", 1), ("/Root/Folder1/Item1", 15), ("/Root/Folder1/Item2", 25),
                ("/Root/Folder2", 2), ("/Root/Folder2/Folder3", 3), ("/Root/Folder2/Folder3/Item3", 5)
            };

            var trie = new PathTree<int>("Root", -1);

            foreach (var item in testTreeChildren)
                trie.AddAsPath(item.Item1, item.Item2);

            return trie;
        }

        public static IEnumerable<TestCaseData> AddCases()
        {
            var rootItemList = new List<(string, int)> { ("/Root", -1) };
            yield return new TestCaseData(rootItemList);

            var singleItemList = new List<(string, int)> { ("/Root", -1), ("/Root/Folder1", 1) };
            yield return new TestCaseData(singleItemList);

            var nestedList = new List<(string, int)> {
                ("/Root", -1),
                ("/Root/Folder1", 1),
                ("/Root/Folder1/Item1", 15),
                ("/Root/Folder1/Item2", 25),
                ("/Root/Folder2", 2),
                ("/Root/Folder2/Folder3", 3),
                ("/Root/Folder2/Folder3/Item3", 5)
            };
            yield return new TestCaseData(nestedList);
        }

        public static IEnumerable<TestCaseData> AddDuplicateCases()
        {
            var items = new List<(string, int)> { ("/Root", -1), ("/Root/Folder1", 1), ("/Root/Folder1/Item1", 15) };
            var rootDuplicate = ("/Root/Folder1", 1);

            yield return new TestCaseData(items, rootDuplicate);

            var childDuplicate = ("/Root/Folder1/Item1", 15);

            yield return new TestCaseData(items, childDuplicate);
        }

        public static IEnumerable<TestCaseData> EnumerateDepthFirstCases()
        {
            var rootOnlyList = new List<(string, int)> { };
            var rootItemExpected = new List<(string, int)> { ("/Root", -1) };
            yield return new TestCaseData(rootOnlyList, rootItemExpected);

            var singleItemList = new List<(string, int)> { ("/Root/Folder1", 1) };
            var singleItemExpected = new List<(string, int)> { ("/Root", -1), ("/Root/Folder1", 1) };
            yield return new TestCaseData(singleItemList, singleItemExpected);

            var nestedItemList = new List<(string, int)>
            {
                ("/Root/Folder1", 1),
                ("/Root/Folder1/Folder2", 2),
                ("/Root/Folder1/Folder2/Item3", 5),
            };
            var nestedItemExpected = new List<(string, int)>
            {
                ("/Root", -1),
                ("/Root/Folder1", 1),
                ("/Root/Folder1/Folder2", 2),
                ("/Root/Folder1/Folder2/Item3", 5),
            };
            yield return new TestCaseData(nestedItemList, nestedItemExpected);

            var manyItemList = new List<(string, int)>
            {
                ("/Root/Folder2", 22),
                ("/Root/Folder2/ItemB", 27),
                ("/Root/Folder2/ItemA", 17),
                ("/Root/Folder1", 11),
                ("/Root/Folder1/Folder3", 3),
                ("/Root/Folder1/Folder3/Item3", 35)
            };
            var manyItemExpected = new List<(string, int)>
            {
                ("/Root", -1),
                ("/Root/Folder1", 11),
                ("/Root/Folder1/Folder3", 3),
                ("/Root/Folder1/Folder3/Item3", 35),
                ("/Root/Folder2", 22),
                ("/Root/Folder2/ItemA", 17),
                ("/Root/Folder2/ItemB", 27)
            };
            yield return new TestCaseData(manyItemList, manyItemExpected);
        }

        public static IEnumerable<TestCaseData> PathKeyCases()
        {
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder1", "/Root/Folder1");
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder1/Item2/", "/Root/Folder1/Item2");
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder2/Folder3/Item3", "/Root/Folder2/Folder3/Item3");
        }

        public static IEnumerable<TestCaseData> TryGetValueCases()
        {
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder2", 2);
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder2", 2);
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder2/Folder3/Item3", 5);
            yield return new TestCaseData(BuildTestTree(), "/Root/Folder2/Folder3/Item3", 5);

        }
    }
}
