using System.Collections.Generic;
using NUnit.Framework;

namespace Monaco.PathTree.UnitTests;

public class PathTreeTestCases
{
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
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder1", "/Root/Folder1");
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder1/Item2/", "/Root/Folder1/Item2");
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder2/Folder3/Item3", "/Root/Folder2/Folder3/Item3");
    }

    public static IEnumerable<TestCaseData> TryGetItemCases()
    {
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root", -1);
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder2", 2);
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder2", 2);
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder2/Folder3/Item3", 5);
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder2/Folder3/Item3", 5);
    }

    public static IEnumerable<TestCaseData> RemoveNodeCases()
    {
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder1/Item2");
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), "/Root/Folder1");
    }

    public static IEnumerable<TestCaseData> CountCases()
    {
        yield return new TestCaseData(TestTreeBuilder.BuildMultiLayerTree(), 7);
        yield return new TestCaseData(TestTreeBuilder.BuildRootOnlyTree(), 1);
    }
}
