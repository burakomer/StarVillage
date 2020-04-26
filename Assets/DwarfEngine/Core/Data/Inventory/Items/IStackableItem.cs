using System;
using System.Collections.Generic;

namespace DwarfEngine
{
    public interface IStackableItem
    {
        int count { get; set; }
        int maxStack { get; }
    }
}
