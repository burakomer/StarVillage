using UnityEngine;

namespace DwarfEngine
{
    public interface IProgressSource
    {
        string targetBar { get; }
        Vector3 barOffset { get; }
    }
}
