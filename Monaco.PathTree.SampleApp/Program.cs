using System;
using System.Linq;

namespace Monaco.PathTree.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new PathTree<Resource, Metadata>("Root",
                new StringResource("Root Resource", "This is the Root Resource"),
                new Metadata(DateTime.Now, Guid.NewGuid())
            );

            tree.AddItemAsPath("/Root/Node1", 
                new StringResource("Resource 1a", "This is the first child node"),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(1), Guid.NewGuid())
            );

            tree.AddItemAsPath("/Root/Node2",
                new StringResource("Resource 1b", "This is the second child node"),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(2), Guid.NewGuid())
            );

            tree.AddItemAsPath("/Root/Node1/SlateBlueNode",
                new Rgba32Resource("Slate Blue", 113, 113, 198, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(3), Guid.NewGuid())
            );

            tree.AddItemAsPath("/Root/Node1/DarkKhakiNode",
                new Rgba32Resource("Dark Khaki", 189, 183, 107, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(4), Guid.NewGuid())
            );

            tree.AddItemAsPath("/Root/Node2/BlueNode",
                new Rgba32Resource("Blue", 0, 0, 255, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(5), Guid.NewGuid())
            );

            tree.AddItemAsPath("/Root/Node2/WhiteNode",
                new Rgba32Resource("White", 255, 255, 255, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(6), Guid.NewGuid())
            );

            foreach (var node in tree.EnumerateBreadthFirst())
            {
                var output = node.Item switch
                {
                    StringResource stringResource => $"'{stringResource.Name}': '{stringResource.Contents}'",
                    Rgba32Resource rgba32Resource => $"'{rgba32Resource.Name}': RGBA ({rgba32Resource.R}, {rgba32Resource.G}, {rgba32Resource.B}, {rgba32Resource.A})",
                    Resource resource => $"'{resource.Name}'"
                };

                Console.WriteLine($"{node.PathKey} : '{node.Name}'\n\tItem: {output};\n\tMetadata: {node.Metadata.CreationTime}, {node.Metadata.Guid}");
            }
        }
    }
}
