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

            foreach (var node in tree.EnumerateBreadthFirst())
            {
                var output = node.Item switch
                {
                    StringResource stringResource => $"{node.PathKey} : '{node.Name}'; '{stringResource.Name}': '{stringResource.Contents}'",
                    Resource resource => $"{node.PathKey} : '{node.Name}'; '{resource.Name}'"
                };

                Console.WriteLine($"{output} | {node.Metadata.CreationTime}, {node.Metadata.Guid}");
            }
        }
    }
}
