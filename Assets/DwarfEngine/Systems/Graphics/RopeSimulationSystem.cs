using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace DwarfEngine
{
    //[UpdateInGroup(typeof(FixedUpdateGroup))]
    public class RopeSimulationSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.fixedDeltaTime;
            JobHandle jobHandle = Entities.ForEach((ref DynamicBuffer<RopeSegmentBufferElement> ropeSegments, in RopeSimulationData simData) =>
            {
                // SIMULATION
                float2 forceGravity = simData.gravityPoint - simData.refPoint;

                for (int i = 0; i < simData.segmentCount; i++)
                {
                    RopeSegment firstSegment = ropeSegments[i].value;
                    float2 velocity = firstSegment.posNow - firstSegment.posOld;
                    firstSegment.posOld = firstSegment.posNow;
                    firstSegment.posNow += velocity;
                    firstSegment.posNow += (forceGravity * simData.gravityStrength) * deltaTime;
                    ropeSegments[i] = new RopeSegmentBufferElement { value = firstSegment }; 
                }

                // CONSTRAINTS
                for (int iteration = 0; iteration < 50; iteration++)
                {
                    //Constrant to Start Point 
                    RopeSegment firstSegment = ropeSegments[0].value;
                    firstSegment.posNow = simData.startPoint;
                    ropeSegments[0] = new RopeSegmentBufferElement { value = firstSegment };

                    //Constrant to End Point 
                    RopeSegment endSegment = ropeSegments[simData.segmentCount - 1].value;
                    endSegment.posNow = simData.endPoint;
                    ropeSegments[ropeSegments.Length - 1] = new RopeSegmentBufferElement { value = endSegment };


                    for (int i = 0; i < simData.segmentCount - 1; i++)
                    {
                        RopeSegment firstSeg = ropeSegments[i].value;
                        RopeSegment secondSeg = ropeSegments[i + 1].value;

                        float dist = math.sqrt(math.pow(firstSeg.posNow.x - secondSeg.posNow.x, 2)
                            + math.pow(firstSeg.posNow.y - secondSeg.posNow.y, 2)); // Magnitude calculation

                        float error = math.abs(dist - simData.ropeSegLen);
                        float2 changeDir = float2.zero;

                        if (dist > simData.ropeSegLen)
                        {
                            changeDir = math.normalize(firstSeg.posNow - secondSeg.posNow);
                        }
                        else if (dist < simData.ropeSegLen)
                        {
                            changeDir = math.normalize(secondSeg.posNow - firstSeg.posNow);
                        }

                        float2 changeAmount = changeDir * error;
                        if (i != 0)
                        {
                            firstSeg.posNow -= changeAmount * 0.5f;
                            ropeSegments[i] = new RopeSegmentBufferElement { value = firstSeg }; ;
                            secondSeg.posNow += changeAmount * 0.5f;
                            ropeSegments[i + 1] = new RopeSegmentBufferElement { value = secondSeg };
                        }
                        else
                        {
                            secondSeg.posNow += changeAmount;
                            ropeSegments[i + 1] = new RopeSegmentBufferElement { value = secondSeg };
                        } 
                    }
                }

            }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}
