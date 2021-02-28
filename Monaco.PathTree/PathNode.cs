using System;
using System.Linq;
using System.Collections.Generic;
using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public class PathNode<TItem> : PathNodeBase<PathNode<TItem, EmptyMetadata>, TItem, EmptyMetadata>
    {
        public PathNode(string rootNodeName, TItem item, EmptyMetadata metadata = default) :
            base(rootNodeName, item, metadata)
        {
        }

        protected override PathNode<TItem, EmptyMetadata> CreateNode(string nodeName, TItem item, EmptyMetadata metadata = default)
        {
            return new PathNode<TItem, EmptyMetadata>(nodeName, item, metadata);
        }
    }

    public sealed class PathNode<TItem, TMetadata> : PathNodeBase<PathNode<TItem, TMetadata>, TItem, TMetadata>
    {
        public PathNode(string rootNodeName, TItem item, TMetadata metadata = default) :
            base(rootNodeName, item, metadata)
        {
        }

        protected override PathNode<TItem, TMetadata> CreateNode(string nodeName, TItem item, TMetadata metadata = default)
        {
            return new PathNode<TItem, TMetadata>(nodeName, item, metadata);
        }
    }
}
