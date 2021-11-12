using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree.ConsoleSample;

public class ResourceTree : PathTreeBase<ResourceNode, Resource>
{
    public ResourceTree(ResourceNode root) :
        base(root)
    {
    }
}
