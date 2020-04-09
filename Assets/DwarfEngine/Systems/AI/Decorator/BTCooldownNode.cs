using System.Collections;
using UnityEngine;

namespace DwarfEngine.AI
{
    /// <summary>
    /// Returns failure while cooldown is recovering.
    /// </summary>
    public class BTCooldownNode : BTDecoratorNode
    {
        public float cooldownTime;

        private float cooldownTimer;
        private Coroutine _cooldownCoroutine;

        protected override BTNodeState DecoratedNode(GameObject owner)
        {
            if (_cooldownCoroutine != null)
            {
                _cooldownCoroutine = GameManager.Instance.StartCoroutine(Cooldown());
                return childNode.Tick(owner);
            }
            else
            {
                return BTNodeState.Failure;
            }
        }

        public IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(cooldownTime);
            _cooldownCoroutine = null;
        }
    }
}