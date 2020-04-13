using Malee;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DwarfEngine
{
    public class StatsManager : MonoBehaviour
    {
        [Reorderable] public StatList stats;

        private Dictionary<string, Stat> statDictionary; 

        private void Start()
        {
            statDictionary = new Dictionary<string, Stat>();

            foreach (Stat stat in stats)
            {
                statDictionary.Add(stat.name, stat);
            }

            MonoBehaviour[] components = GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour comp in components)
            {
                Type compType = comp.GetType();

                FieldInfo[] fieldInfos = compType.GetFields();

                foreach (var field in fieldInfos)
                {
                    StatIdAttribute statId = (StatIdAttribute)Attribute.GetCustomAttribute(field, typeof(StatIdAttribute));

                    if (statId != null)
                    {
                        // TODO : Instead of directly setting it, subscribe it to an observable.
                        field.SetValue(comp, (int)statDictionary[statId.statId].value);
                    }
                }
            }
        }
    }
}