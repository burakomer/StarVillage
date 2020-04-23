using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DwarfEngine
{
    public class FixedUpdateSystem : MonoBehaviour
    {
        private IEnumerable<ComponentSystemBase> simSystems;

        private void Start()
        {
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<FixedUpdateGroup>().Enabled = false;
            simSystems = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<FixedUpdateGroup>().Systems;
        }

        private void FixedUpdate()
        {
            foreach (var sys in simSystems)
            {
                sys.Update();
            }
        }
    }
}