using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public enum ResourceType { Inventory, Character }

    public class WeaponResource : WeaponComponent
    {
        public ResourceType resourceType;
        public string resourceName;
        public int consumeAmount;

        //private Inventory _inventory;
        private CharacterResource _resource;

        protected override void Init()
        {
            base.Init();

            if (resourceType == ResourceType.Character)
            {
                List<CharacterResource> resources = new List<CharacterResource>();
                weapon.owner.GetComponents<CharacterResource>(resources);
                _resource = resources.Find(r => r.resourceName == resourceName);
            }
        }

        public bool Consume()
        {
            switch (resourceType)
            {
                case ResourceType.Inventory:
                    // Check players inventory if the item amount requested exists. If it does, reduce it by consumeAmount
                    return false;
                case ResourceType.Character:
                    return _resource.Consume(consumeAmount);
            }
            return false;
        }

        public bool CheckResource()
        {
            switch (resourceType)
            {
                case ResourceType.Inventory:
                    // Check players inventory if the item amount requested exists. Return the check.
                    break;
                case ResourceType.Character:
                    return _resource.CheckResource(consumeAmount);
            }
            return false;
        }
    }
}
