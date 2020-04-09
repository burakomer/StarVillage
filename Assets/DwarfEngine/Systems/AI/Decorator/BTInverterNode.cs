using UnityEngine;

namespace DwarfEngine.AI
{
    [CreateAssetMenu(fileName = "New Inverter Node", menuName = "TouchRPGUltimate/AI/Decorator Nodes/Inverter Node")]
    public class BTInverterNode : BTDecoratorNode
    {
        protected override BTNodeState DecoratedNode(GameObject owner)
        {
            BTNodeState state = childNode.Tick(owner);
            if (state != BTNodeState.Running)
            {
                if (state == BTNodeState.Success)
                {
                    return BTNodeState.Failure;
                }
                else
                {
                    return BTNodeState.Success;
                }
            }
            else
            {
                return BTNodeState.Running;
            }
        }
    }
}