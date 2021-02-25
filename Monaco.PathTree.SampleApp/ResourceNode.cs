using System;
using System.Collections.Generic;
using System.Text;
using Monaco.PathTree;

namespace Monaco.PathTree.SampleApp
{
    public class ResourceNode : PathTreeNode<Resource>
    {
        public ResourceNode(string name, Resource value) : base(name, value)
        {
        }
    }
}
