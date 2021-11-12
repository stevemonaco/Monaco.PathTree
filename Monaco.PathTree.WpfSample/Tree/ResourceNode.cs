using Monaco.PathTree.Abstractions;
using Monaco.PathTree.Samples.Wpf.DomainModels;

namespace Monaco.PathTree.Samples.Wpf.Tree;

public class ResourceNode : PathNodeBase<ResourceNode, IOrgResource>
{
    public ResourceNode(string name, IOrgResource item) : base(name, item)
    {
    }
}
