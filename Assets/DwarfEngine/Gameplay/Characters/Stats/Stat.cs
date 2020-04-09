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
    public abstract class Stat<T>
    {
        public T BaseValue { get => baseValue; }
        public Dictionary<GameObject, T> modifierSources;
        
        [SerializeField] protected T baseValue;
        public abstract T value { get; }

        public event StatEvent<T> StatUpdated;
        
        public abstract void ModifyStat(GameObject modifierSource, T modifyValue, StatModifyType modifyType);

        public void ChangeBaseValue(T newBaseValue)
        {
            baseValue = newBaseValue;
            StatUpdated?.Invoke(value);
        }

        public void OnStatUpdated()
        {
            StatUpdated?.Invoke(value);
        }
    }

    [Serializable]
    public abstract class Stat<T1, T2> : Stat<T1>
    {
        public Dictionary<GameObject, T2> secondModifierSources;
        
        public abstract void ModifyStat(GameObject modifierSource, T2 modifyValue);
    }

    [Serializable]
    public class NumberStat : Stat<int, float>
    {
        public override int value 
        { 
            get 
            {
                int additiveModifier = 0;
                foreach (int modifier in modifierSources.Values)
                {
                    additiveModifier += modifier;
                }
                float multiplicativeModifier = 1f;
                foreach (float modifier in secondModifierSources.Values)
                {
                    multiplicativeModifier += modifier;
                }

                return Mathf.RoundToInt((baseValue * multiplicativeModifier) + additiveModifier); 
            } 
        }
        
        public override void ModifyStat(GameObject modifierSource, int modifyValue, StatModifyType modifyType)
        {
            switch (modifyType)
            {
                case StatModifyType.Add:
                    if (modifierSources.ContainsKey(modifierSource))
                    {
                        modifierSources[modifierSource] = modifyValue;
                    }
                    else
                    {
                        modifierSources.Add(modifierSource, modifyValue);
                    }
                    break;
                case StatModifyType.Remove:
                    if (modifierSources.ContainsKey(modifierSource))
                    {
                        modifierSources.Remove(modifierSource);
                    }
                    break;
            }
            
            OnStatUpdated();
        }

        public override void ModifyStat(GameObject modifierSource, float modifyValue)
        {
            if (secondModifierSources.ContainsKey(modifierSource))
            {
                secondModifierSources[modifierSource] = modifyValue;
            }
            else
            {
                secondModifierSources.Add(modifierSource, modifyValue);
            }
            OnStatUpdated();
        }
    }
    
    [Serializable]
    public class EnumStat<T> : Stat<T> where T : Enum
    {
        public override T value => (T)(modifier ?? baseValue);
        public Enum modifier;

        public override void ModifyStat(GameObject modifierSource, T modifyValue, StatModifyType modifyType)
        {
            switch (modifyType)
            {
                case StatModifyType.Add:
                    modifier = modifyValue;
                    break;
                case StatModifyType.Remove:
                    modifier = null;
                    break;
            }
            OnStatUpdated();
        }
    }

    //public enum DamageTypes { Normal, Magical, Physical, True }
    //public class DamageTypesStat : EnumStat<DamageTypes>
    //{
    //
    //}
}
