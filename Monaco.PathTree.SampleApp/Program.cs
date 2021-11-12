using System;

namespace Monaco.PathTree.ConsoleSample;

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
        var rootNode = new PathNode<Resource>("Root",
            new StringResource("Root Resource", "This is the Root Resource"));

        var tree = new PathTree<PathNode<Resource>, Resource>(rootNode);

        // Add nodes using absolute paths
        tree.AttachNodeAsPath("/Root/Node1",
            new PathNode<Resource>("Node1", new StringResource("Resource 1a", "This is the first child node")));

        var node2 = new PathNode<Resource>("Node2", new StringResource("Resouce 1b", "This is the second child node"));
        tree.AttachNodeAsPath("/Root/Node2", node2);

        tree.AttachNodeToPath("/Root/Node1/",
            new PathNode<Resource>("SlateBlueNode", new Rgba32Resource("Slate Blue", 113, 113, 198, 0)));

        tree.AttachNodeAsPath("/Root/Node1/DarkKhakiNode",
             new PathNode<Resource>("DarkKhakiNode", new Rgba32Resource("Dark Khaki", 189, 183, 107, 0)));

        // Add child nodes directly to a node
        node2.AttachChildNode(new PathNode<Resource>("BlueNode",
            new Rgba32Resource("Blue", 0, 0, 255, 0)));

        node2.AttachChildNode(new PathNode<Resource>("WhiteNode",
            new Rgba32Resource("White", 255, 255, 255, 0)));

        // Print out all nodes
        foreach (var node in tree.EnumerateBreadthFirst())
        {
            var itemOutput = node.Item switch
            {
                StringResource stringResource => $"'{stringResource.Name}': '{stringResource.Contents}'",
                Rgba32Resource rgba32Resource => $"'{rgba32Resource.Name}': RGBA ({rgba32Resource.R}, {rgba32Resource.G}, {rgba32Resource.B}, {rgba32Resource.A})",
                Resource resource => $"'{resource.Name}'"
            };

            Console.WriteLine($"{tree.CreatePathKey(node)} : '{node.Name}'");
            Console.WriteLine($"\tItem: {itemOutput}");
        }
    }

    public static void CustomNodeSample()
    {
        var rootNode = new ResourceNode("Root",
            new StringResource("Root Resource", "This is the Root Resource"),
            DateTime.Now);

        var tree = new ResourceTree(rootNode);

        // Using the built-in PathTree<...> with custom nodes is also an option
        // var tree = new PathTree<ResourceNode, Resource>(rootNode);

        // Add nodes by creating nodes and attaching them
        var node1 = new StringNode("Node1",
            new StringResource("Resource 1a", "This is the first child node"),
            DateTime.Now + TimeSpan.FromMinutes(1));
        node1.NodeCountBeforeAddition = tree.Count();
        rootNode.AttachChildNode(node1);

        var node2 = new StringNode("Node2",
            new StringResource("Resource 1b", "This is the second child node"),
            DateTime.Now + TimeSpan.FromMinutes(2));
        node2.NodeCountBeforeAddition = tree.Count();
        rootNode.AttachChildNode(node2);

        tree.AttachNodeToPath("/Root/Node1",
            new ResourceNode("SlateBlueNode",
                new Rgba32Resource("Slate Blue", 113, 113, 198, 0),
                DateTime.Now + TimeSpan.FromMinutes(3)));

        tree.AttachNodeToPath("/Root/Node1",
            new ResourceNode("DarkKhakiNode",
                new Rgba32Resource("Dark Khaki", 189, 183, 107, 0),
                DateTime.Now + TimeSpan.FromMinutes(4)));

        node2.AttachChildNode(new ResourceNode("BlueNode",
            new Rgba32Resource("Blue", 0, 0, 255, 0),
            DateTime.Now + TimeSpan.FromMinutes(5)));

        node2.AttachChildNode(new ResourceNode("WhiteNode",
            new Rgba32Resource("White", 255, 255, 255, 0),
            DateTime.Now + TimeSpan.FromMinutes(6)));

        // Print out all nodes
        foreach (var node in tree.EnumerateBreadthFirst())
        {
            var nodeOutput = node switch
            {
                StringNode stringNode => $"{tree.CreatePathKey(node)} : '{node.Name}' '{node.GetType().Name}' '{stringNode.NodeCountBeforeAddition}'",
                ResourceNode resourceNode => $"{tree.CreatePathKey(node)} : '{node.Name}' '{node.GetType().Name}'"
            };

            var itemOutput = node.Item switch
            {
                StringResource stringResource => $"'{stringResource.Name}': '{stringResource.Contents}'",
                Rgba32Resource rgba32Resource => $"'{rgba32Resource.Name}': RGBA ({rgba32Resource.R}, {rgba32Resource.G}, {rgba32Resource.B}, {rgba32Resource.A})",
                Resource resource => $"'{resource.Name}'"
            };

            var metadataOutput = node switch
            {
                StringNode stringNode => $"{node.CreationTime}, {node.Guid}, '{stringNode.NodeCountBeforeAddition}'",
                ResourceNode resourceNode => $"{node.CreationTime}, {node.Guid}"
            };

            Console.WriteLine(nodeOutput);
            Console.WriteLine($"\tItem: {itemOutput}");
            Console.WriteLine($"\tMetadata: {metadataOutput}");
        }
    }
}
