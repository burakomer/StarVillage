using System;

namespace DwarfEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EquipmentSlotAttribute : Attribute
    {
        public Type equipmentType { get; }

        public EquipmentSlotAttribute(Type _equipmentType)
        {
            equipmentType = _equipmentType;
        }
    }
}
