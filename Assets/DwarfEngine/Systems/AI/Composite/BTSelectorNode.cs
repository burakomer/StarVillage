using UnityEngine;

namespace DwarfEngine.AI
{
    [CreateAssetMenu(fileName = "New Selector Node", menuName = "TouchRPGUltimate/AI/Composite Nodes/Selector Node")]
    public class BTSelectorNode : BTCompositeNode
    {
        protected override BTNodeState CheckNodes(GameObject owner)
        {
            foreach (BTBaseNode node in childNodes)
            {
                BTNodeState state = node.Tick(owner);

                if (state != BTNodeState.Failure)
                {
                    return state;
                }
            }

            return BTNodeState.Failure;
        }
    }
}