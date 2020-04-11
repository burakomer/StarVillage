using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfEngine
{
    public interface IObjectPooler
    {
        GameObject objectToPool { get; set; }
        int amountToPool { get; set; }
        bool expandInNeed { get; set; }
    }
}
