using UnityEngine;

namespace DwarfEngine
{
    public static class ObjectPoolerFactory
    {
        public static ObjectPooler CreatePooler(GameObject obj, IObjectPooler actualPooler)
        {
            ObjectPooler pooler = obj.AddComponent<ObjectPooler>();
            pooler.objectToPool = actualPooler.objectToPool;
            pooler.amountToPool = actualPooler.amountToPool;
            pooler.expandInNeed = actualPooler.expandInNeed;
            return pooler;
        }
    }
}
