using System;

namespace Monaco.PathTree.ConsoleSample
{
    public class Metadata
    {
        public DateTime CreationTime { get; }
        public Guid Guid { get; }

        public Metadata(DateTime creationTime, Guid guid)
        {
            CreationTime = creationTime;
            Guid = guid;
        }
    }
}
