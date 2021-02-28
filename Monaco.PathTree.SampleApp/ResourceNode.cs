using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree.SampleApp
{
    public class ResourceNode : PathNodeBase<ResourceNode, Resource, Metadata>
    {
        public ResourceNode(string nodeName, Resource resource, Metadata metadata = default) :
            base(nodeName, resource, metadata)
        {
        }

        protected override ResourceNode CreateNode(string nodeName, Resource item, Metadata metadata = null)
        {
            return new ResourceNode(nodeName, item, metadata);
        }
    }
}
