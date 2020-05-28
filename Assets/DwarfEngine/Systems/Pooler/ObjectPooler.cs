using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// A general-purpose object pooler for GameObjects.
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        public event System.Action<GameObject> UpdatePool;

        public GameObject objectToPool;
        public int amountToPool;
        public bool expandInNeed;

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

    /// <summary>
    /// Generic and non-component version of ObjectPooler.
    /// </summary>
    /// <typeparam name="T">Type of the pooled object.</typeparam>
    public class ObjectPooler<T> : IEnumerable where T : MonoBehaviour
    {
        public event System.Action<T> UpdatePool;

        public T objectToPool;
        public int amountToPool;
        public bool expandInNeed;

        private List<T> pooledObjects;
        private Transform _container;

        public ObjectPooler(T _objectToPool, int _amountToPool, bool _expandInNeed)
        {
            objectToPool = _objectToPool;
            amountToPool = _amountToPool;
            expandInNeed = _expandInNeed;
        }

        public void Initialize(System.Action<T> OnUpdatePool, int layer = 0, Transform transformToParent = null)
        {
            UpdatePool += OnUpdatePool;
            _container = new GameObject("Pooled Objects").transform;
            _container.gameObject.layer = layer;

            if (transformToParent != null)
            {
                _container.transform.SetParent(transformToParent);
            }

            pooledObjects = new List<T>();
            for (int i = 0; i < amountToPool; i++)
            {
                T obj = Object.Instantiate(objectToPool);
                obj.transform.SetParent(_container.transform);
                obj.gameObject.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        public T Get()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                T pooledObj = pooledObjects[i];
                if (!pooledObj.gameObject.activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            if (expandInNeed)
            {
                T obj = Object.Instantiate(objectToPool);
                UpdatePool?.Invoke(obj);
                obj.transform.SetParent(_container.transform);
                obj.gameObject.SetActive(false);
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

        /// <summary>
        /// Call this in OnDestroy of MonoBehaviour.
        /// </summary>
        public void Destroy()
        {
            Object.Destroy(_container.gameObject);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)pooledObjects).GetEnumerator();
        }
    }
}