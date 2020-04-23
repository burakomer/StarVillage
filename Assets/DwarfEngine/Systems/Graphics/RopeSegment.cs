using Unity.Mathematics;

namespace DwarfEngine
{
    public struct RopeSegment
    {
        public float2 posNow;
        public float2 posOld;

        public RopeSegment(float2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
