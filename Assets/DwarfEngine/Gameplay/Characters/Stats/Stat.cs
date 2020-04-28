using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public delegate void StatEvent<T>(T stat);

    public enum StatModifyType
    {
        Add,
        Remove
    }
    
    [Serializable]
    public class Stat
    {
        [SerializeField] private float baseValue;
        public float BaseValue { get => baseValue; }

        public Dictionary<GameObject, float> flatModifiers = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> percentageModifiers = new Dictionary<GameObject, float>();

        private bool isDirtyInt = true;
        private bool isDirtyFloat = true;

        private int _intValue;
        public int IntValue
        {
            get
            {
                if (isDirtyInt)
                {
                    _intValue = Mathf.RoundToInt(CalculateStat());
                    isDirtyInt = false;
                }

                return _intValue;
            }
        }

        private float _floatValue;
        public float FloatValue
        {
            get
            {
                if (isDirtyFloat)
                {
                    _floatValue = CalculateStat();
                    isDirtyFloat = false;
                }

                return _floatValue;
            }
        }

        private float CalculateStat()
        {
            int flatModifier = 0;
            foreach (int modifier in flatModifiers.Values)
            {
                flatModifier += modifier;
            }
            float percModifier = 0f;
            foreach (float modifier in percentageModifiers.Values)
            {
                percModifier += modifier;
            }

            return BaseValue + flatModifier + ((BaseValue + flatModifier) * (percModifier / 100));
        }

        /// <summary>
        /// Add/Remove flat modifier.
        /// </summary>
        public void ModifyStat(GameObject modifierSource, int modifyValue, StatModifyType modifyType)
        {
            switch (modifyType)
            {
                case StatModifyType.Add:
                    if (flatModifiers.ContainsKey(modifierSource))
                    {
                        flatModifiers[modifierSource] = modifyValue;
                    }
                    else
                    {
                        flatModifiers.Add(modifierSource, modifyValue);
                    }
                    break;
                case StatModifyType.Remove:
                    if (flatModifiers.ContainsKey(modifierSource))
                    {
                        flatModifiers.Remove(modifierSource);
                    }
                    break;
            }

            isDirtyFloat = true;
            isDirtyInt = true;
        }

        /// <summary>
        /// Add/Remove percentage modifier
        /// </summary>
        public void ModifyStat(GameObject modifierSource, float modifyValue, StatModifyType modifyType)
        {
            switch (modifyType)
            {
                case StatModifyType.Add:
                    if (percentageModifiers.ContainsKey(modifierSource))
                    {
                        percentageModifiers[modifierSource] = modifyValue;
                    }
                    else
                    {
                        percentageModifiers.Add(modifierSource, modifyValue);
                    }
                    break;
                case StatModifyType.Remove:
                    if (percentageModifiers.ContainsKey(modifierSource))
                    {
                        percentageModifiers.Remove(modifierSource);
                    }
                    break;
            }

            isDirtyFloat = true;
            isDirtyInt = true;
        }
    }
}
