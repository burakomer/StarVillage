using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// An object pooler for GameObjects.
    /// </summary>
    public class ObjectPooler : MonoBehaviour, IObjectPooler
    {
        public event Action<GameObject> UpdatePool;
        
        public GameObject objectToPool { get; set; }
        public int amountToPool { get; set; }
        public bool expandInNeed { get; set; }

        public List<GameObject> pooledObjects;
        private GameObject _container;

        public void Initialization(bool setParentToThis, int layer = 0)
        {
            _container = new GameObject("Pooled Objects");
            _container.layer = layer;

            if (setParentToThis)
            {
                _container.transform.SetParent(transform);
            }

            pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(objectToPool);
                obj.transform.SetParent(_container.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        public T GetPooledObject<T>() where T : MonoBehaviour
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                GameObject pooledObj = pooledObjects[i];
                if (!pooledObj.activeInHierarchy)
                {
                    return pooledObjects[i].GetComponent<T>();
                }
            }
            if (expandInNeed)
            {
                GameObject obj = Instantiate(objectToPool);
                UpdatePool?.Invoke(obj);
                obj.transform.SetParent(_container.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }

        private void OnDestroy()
        {
            Destroy(_container);
        }
    }
}