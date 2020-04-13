using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [AttributeUsage(AttributeTargets.Field,Inherited = false, AllowMultiple = true)]
    public sealed class StatIdAttribute : Attribute
    {
        public string statId { get; }

        public StatIdAttribute(string _statId)
        {
            statId = _statId;
        }
    }
}
