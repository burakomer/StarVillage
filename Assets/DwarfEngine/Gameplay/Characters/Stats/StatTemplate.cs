using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace DwarfEngine
{
    public interface IStatModel
    {
        string name { get; set; }
    }

    public interface IStat : IStatModel
    {
        float baseValue { get; }
    }


    [Serializable]
    public class StatTemplate
    {
        public string name;
        public float minValue;
        public float maxValue;
        public Type type;
    }

    [Serializable]
    public class DynamicStat
    {
        public string name;
        //public StatModifierType type;
        public float value;
    }

    public enum StatModifierType { Flat, Percentage }
}
