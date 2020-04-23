using Unity.Entities;

namespace DwarfEngine
{
    [InternalBufferCapacity(35)]
    public struct RopeSegmentBufferElement : IBufferElementData
    {
        public RopeSegment value;
    }
}
