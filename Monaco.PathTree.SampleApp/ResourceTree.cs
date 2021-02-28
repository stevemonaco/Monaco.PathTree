using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree.SampleApp
{
    public class ResourceTree : PathTreeBase<ResourceNode, Resource, Metadata>
    {
        public ResourceTree(ResourceNode root) :
            base(root)
        {
        }
    }
}
