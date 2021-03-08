﻿using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
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
