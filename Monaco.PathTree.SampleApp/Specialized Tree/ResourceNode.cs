using Monaco.PathTree.Abstractions;
using System;

namespace Monaco.PathTree.ConsoleSample;

public class ResourceNode : PathNodeBase<ResourceNode, Resource>
{
    public DateTime CreationTime { get; }
    public Guid Guid { get; }

    public ResourceNode(string nodeName, Resource resource, DateTime creationTime) :
        base(nodeName, resource)
    {
        CreationTime = creationTime;
        Guid = Guid.NewGuid();
    }
}
