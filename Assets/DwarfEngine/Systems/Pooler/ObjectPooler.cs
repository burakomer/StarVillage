using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// An object pooler for GameObjects.
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        public event Action<GameObject> UpdatePool;

        public GameObject objectToPool;
        public int amountToPool;
        public bool expandInNeed;

        public List<GameObject> pooledObjects;
        [HideInInspector] public GameObject _container;

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

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                GameObject pooledObj = pooledObjects[i];
                if (!pooledObj.activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            if (expandInNeed)
            {
                GameObject obj = Instantiate(objectToPool);
                UpdatePool?.Invoke(obj);
                obj.transform.SetParent(_container.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Use to re-attach a detached pooled object.
        /// </summary>
        public void SetParentToContainer(Transform obj)
        {
            obj.SetParent(_container.transform);
        }

        private void OnDestroy()
        {
            Destroy(_container);
        }
    }
}