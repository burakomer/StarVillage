using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfEngine
{
    public interface IActiveEquipmentSlot
    {
        /// <summary>
        /// Call the equipments activation method here.
        /// </summary>
        void Use(IActiveEquipment equipment);
    }
}
