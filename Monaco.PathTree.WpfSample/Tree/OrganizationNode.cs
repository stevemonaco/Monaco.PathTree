using Monaco.PathTree.Samples.Wpf.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.PathTree.Samples.Wpf.Tree
{
    public class OrganizationNode : ResourceNode
    {
        public OrganizationNode(string name, IOrgResource item) : base(name, item)
        {
        }
    }
}
