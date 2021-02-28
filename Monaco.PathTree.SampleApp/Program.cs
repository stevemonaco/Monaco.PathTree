using System;
using System.Linq;

namespace Monaco.PathTree.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---- Running built-in node sample ----");
            BuiltInNodeSample();

            Console.WriteLine("\n---- Running custom node sample ----");
            CustomNodeSample();
        }

        public static void BuiltInNodeSample()
        {
            var rootNode = new PathNode<Resource, Metadata>("Root",
                new StringResource("Root Resource", "This is the Root Resource"),
                new Metadata(DateTime.Now, Guid.NewGuid()));

            var tree = new PathTree<PathNode<Resource, Metadata>, Resource, Metadata>(rootNode);

            // Add nodes using absolute paths
            tree.AddItemAsPath("/Root/Node1",
                new StringResource("Resource 1a", "This is the first child node"),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(1), Guid.NewGuid())
            );

            var node2 = tree.AddItemAsPath("/Root/Node2",
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

            // Add child nodes directly to a node
            node2.AddChild("BlueNode",
                new Rgba32Resource("Blue", 0, 0, 255, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(5), Guid.NewGuid())
            );

            node2.AddChild("WhiteNode",
                new Rgba32Resource("White", 255, 255, 255, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(6), Guid.NewGuid())
            );

            // Print out all nodes
            foreach (var node in tree.EnumerateBreadthFirst())
            {
                var itemOutput = node.Item switch
                {
                    StringResource stringResource => $"'{stringResource.Name}': '{stringResource.Contents}'",
                    Rgba32Resource rgba32Resource => $"'{rgba32Resource.Name}': RGBA ({rgba32Resource.R}, {rgba32Resource.G}, {rgba32Resource.B}, {rgba32Resource.A})",
                    Resource resource => $"'{resource.Name}'"
                };

                Console.WriteLine($"{node.PathKey} : '{node.Name}'");
                Console.WriteLine($"\tItem: {itemOutput}");
                Console.WriteLine($"\tMetadata: {node.Metadata.CreationTime}, {node.Metadata.Guid}");
            }
        }

        public static void CustomNodeSample()
        {
            var rootNode = new ResourceNode("Root",
                new StringResource("Root Resource", "This is the Root Resource"),
                new Metadata(DateTime.Now, Guid.NewGuid()));

            var tree = new ResourceTree(rootNode);

            // Using the built-in PathTree<...> with custom nodes is also an option
            //var tree = new PathTree<ResourceNode, Resource, Metadata>(rootNode);

            // Add nodes using absolute paths
            tree.AddItemAsPath("/Root/Node1",
                new StringResource("Resource 1a", "This is the first child node"),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(1), Guid.NewGuid())
            );

            var node2 = tree.AddItemAsPath("/Root/Node2",
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

            // Add child nodes directly to a node
            node2.AddChild("BlueNode",
                new Rgba32Resource("Blue", 0, 0, 255, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(5), Guid.NewGuid())
            );

            node2.AddChild("WhiteNode",
                new Rgba32Resource("White", 255, 255, 255, 0),
                new Metadata(DateTime.Now + TimeSpan.FromMinutes(6), Guid.NewGuid())
            );

            // Print out all nodes
            foreach (var node in tree.EnumerateBreadthFirst())
            {
                var itemOutput = node.Item switch
                {
                    StringResource stringResource => $"'{stringResource.Name}': '{stringResource.Contents}'",
                    Rgba32Resource rgba32Resource => $"'{rgba32Resource.Name}': RGBA ({rgba32Resource.R}, {rgba32Resource.G}, {rgba32Resource.B}, {rgba32Resource.A})",
                    Resource resource => $"'{resource.Name}'"
                };

                Console.WriteLine($"{node.PathKey} : '{node.Name}'");
                Console.WriteLine($"\tItem: {itemOutput}");
                Console.WriteLine($"\tMetadata: {node.Metadata.CreationTime}, {node.Metadata.Guid}");
            }
        }
    }
}
