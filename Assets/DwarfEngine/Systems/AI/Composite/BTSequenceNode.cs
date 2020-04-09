using UnityEngine;

namespace DwarfEngine.AI
{
    /// <summary>
    /// When a child returns failure or running, it will return that state. Otherwise, it will tick all nodes and returns success when all succeed.
    /// </summary>
    [CreateAssetMenu(fileName = "New Sequence Node", menuName = "TouchRPGUltimate/AI/Composite Nodes/Sequence Node")]
    public class BTSequenceNode : BTCompositeNode
    {
        protected override BTNodeState CheckNodes(GameObject owner)
        {
            foreach (BTBaseNode node in childNodes)
            {
                BTNodeState state = node.Tick(owner);
                
                if (state != BTNodeState.Success)
                {
                    return state;
                }
            }
            
            return BTNodeState.Success;
        }
    }
}