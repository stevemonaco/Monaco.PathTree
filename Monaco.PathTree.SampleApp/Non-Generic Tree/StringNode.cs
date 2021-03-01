using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.PathTree.SampleApp
{
    public class StringNode : ResourceNode
    {
        /// <summary>
        /// Tracks how many nodes were in the tree before the node was added
        /// Just a placeholder sample property
        /// </summary>
        public int NodeCountBeforeAddition { get; set; }

        public StringNode(string nodeName, Resource resource, Metadata metadata = null) 
            : base(nodeName, resource, metadata)
        {
        }
    }
}
