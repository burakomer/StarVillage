using Unity.Entities;
using Unity.Mathematics;

namespace DwarfEngine
{
    public struct RopeSimulationData : IComponentData
    {
        public int segmentCount;
        public float ropeSegLen;
        public float2 startPoint;
        public float2 refPoint;
        public float2 endPoint;
        public float2 gravityPoint;
        public float gravityStrength;
    }
}
