using System;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class ProgressBarBase : MonoBehaviour, IObserver<float>
    {
        [Header("Properties")]
        public string barName;

        public abstract void OnNext(float value);
        
        public virtual void OnCompleted()
        {

        }
        
        public virtual void OnError(Exception error)
        {

        }
    }
}
