using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfEngine
{
    public interface IActiveEquipment
    {
        void StartEquipment();
        void StopEquipment();
    }
}
