using System;
using System.Linq;

namespace Monaco.PathTree.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new ResourceNode("RootNode", new StringResource("Root", "This is the root node"));
            var tree = new PathTree<ResourceNode>(root.Name, root);

            tree.AddAsPath("/RootNode/Node1", new ResourceNode("Node1", new StringResource("Node1", "This is the first child node")));
            tree.AddAsPath("/RootNode/Node2", new ResourceNode("Node2", new StringResource("Node2", "This is the second child node")));

            foreach (var node in tree.EnumerateBreadthFirst())
            {
                var output = node.Value.Value switch
                {
                    StringResource stringResource => $"{node.PathKey} : '{node.Name}', '{stringResource.Name}, '{stringResource.Contents}'",
                    Resource resource => $"{node.PathKey} : '{node.Name}', '{resource.Name}"
                };

                Console.WriteLine(output);
            }
        }
    }
}
