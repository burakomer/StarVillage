using UnityEngine;
using DwarfEngine.AI;

namespace DwarfEngine
{
    //[RequireComponent(typeof(InputManager))]
    public class AIBrain : CharacterBrain
    {
        [Expandable] public BehaviourTree behaviourTree;

        protected override void SetInputSources()
        {
            //inputManager = GetComponent<InputManager>();
        }

        protected override void UpdateBrain()
        {
            base.UpdateBrain();

            behaviourTree.Tick(gameObject);
        }
    }
}