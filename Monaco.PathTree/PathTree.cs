using System;
using System.Collections.Generic;
using System.Linq;
using Monaco.PathTree.Abstractions;

namespace Monaco.PathTree
{
    public sealed class PathTree<TNode, TItem> : PathTreeBase<TNode, TItem, EmptyMetadata>
        where TNode : PathNodeBase<TNode, TItem, EmptyMetadata>
    {
        public PathTree(TNode root) :
            base(root)
        {
        }
    }

    public sealed class PathTree<TNode, TItem, TMetadata> : PathTreeBase<TNode, TItem, TMetadata>
        where TNode : PathNodeBase<TNode, TItem, TMetadata>
    {
        public PathTree(TNode root) :
            base(root)
        {
        }
    }
}
