using System;

namespace Monaco.PathTree.ConsoleSample
{
    public class StringNode : ResourceNode
    {
        /// <summary>
        /// Tracks how many nodes were in the tree before the node was added
        /// Just a placeholder sample property
        /// </summary>
        public int NodeCountBeforeAddition { get; set; }

        public StringNode(string nodeName, Resource resource, DateTime creationTime) 
            : base(nodeName, resource, creationTime)
        {
        }
    }
}
