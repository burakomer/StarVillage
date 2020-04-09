using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine.AI
{
    public abstract class BTCompositeNode : BTBaseNode
    {
        [Expandable] public List<BTBaseNode> childNodes;

        public override BTNodeState Tick(GameObject owner)
        {
            return CheckNodes(owner);
        }

        protected abstract BTNodeState CheckNodes(GameObject owner);
    }

    
}