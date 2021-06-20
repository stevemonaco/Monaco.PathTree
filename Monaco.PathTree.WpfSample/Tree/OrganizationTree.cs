using Monaco.PathTree.Abstractions;
using Monaco.PathTree.Samples.Wpf.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.PathTree.Samples.Wpf.Tree
{
    public sealed class OrganizationTree : PathTreeBase<ResourceNode, IOrgResource>
    {
        public OrganizationTree(ResourceNode root) : base(root)
        {
        }
    }
}
