using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// The manager class that handles stats for its object with the given collection.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    public abstract class StatsManager<TObject, TCollection> : MonoBehaviour where TObject : MonoBehaviour where TCollection : StatsCollection
    {
        public TObject obj;
        public TCollection stats;

        private void Start()
        {
            obj = GetComponent<TObject>();
        }
    }
}
