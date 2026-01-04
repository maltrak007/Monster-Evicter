using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace ProyectoFinalFolder.Components.PoolSystem
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private bool _addToDontDestroyOnLoad = false;

        private GameObject _emptyHolder;

        private static GameObject _particleSystemsEmpty;
        private static GameObject _projectileEmpty;
        private static GameObject _soundFXEmpty;
        private static GameObject _gameObjectEmpty;

        private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
        private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

        public enum PoolType
        {
            ParticleSystems,
            Projectiles,
            SoundFX,
            GameObjects,
        }

        public static PoolType poolingType;

        private void Awake()
        {
            _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
            _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

            SetupEmpties();
        }

        private void SetupEmpties()
        {
            _emptyHolder = new GameObject("Object Pools");
            
            _particleSystemsEmpty = new GameObject("Particle Systems");
            _particleSystemsEmpty.transform.SetParent(_emptyHolder.transform);
            
            _projectileEmpty = new GameObject("Projectiles");
            _projectileEmpty.transform.SetParent(_emptyHolder.transform);
            
            _soundFXEmpty = new GameObject("Sound FX");
            _soundFXEmpty.transform.SetParent(_emptyHolder.transform);
            
            _gameObjectEmpty = new GameObject("GameObjects");
            _gameObjectEmpty.transform.SetParent(_emptyHolder.transform);
            
            //  Optional Code 
            // if (_addToDontDestroyOnLoad)
            // {
            //     DontDestroyOnLoad(_particleSystemsEmpty.transform.root);
            // }
        }
        
        private static void CreatePool (GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(createFunc: () => CreateGameObject(prefab, pos, rot, poolType),
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: OnDestroyObject);
            
            _objectPools.Add(prefab, pool);
        }

        private static GameObject CreateGameObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
        {
            prefab.SetActive(false);
            
            GameObject obj = Instantiate(prefab, pos, rot);
            
            prefab.SetActive(true);

            GameObject parentObject = SetParentObject(poolType);
            obj.transform.SetParent(parentObject.transform);
            return obj;
        }

        private static GameObject SetParentObject(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.Projectiles:
                    return _projectileEmpty;
                case PoolType.ParticleSystems:
                    return _particleSystemsEmpty;
                case PoolType.SoundFX:
                    return _soundFXEmpty;
                case PoolType.GameObjects:
                    return _gameObjectEmpty;
                default:
                    return null;
            }
        }

        private static void OnGetObject(GameObject obj)
        {
            //Optional Logic
        }
        
        private static void OnReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
        }
        
        private static void OnDestroyObject(GameObject obj)
        {
            if (_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Remove(obj);
            }
        }
        
        private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRot, PoolType poolType = PoolType.GameObjects) where T : Object
        {
            if (!_objectPools.ContainsKey(objectToSpawn))
            {
                CreatePool(objectToSpawn, spawnPos, spawnRot, poolType);
            }

            GameObject obj = _objectPools[objectToSpawn].Get();

            if (obj != null)
            {
                if (!_cloneToPrefabMap.ContainsKey(obj))
                {
                    _cloneToPrefabMap.Add(obj, objectToSpawn);    
                }
                
                obj.transform.position = spawnPos;
                obj.transform.rotation = spawnRot;
                obj.SetActive(true);
                
                if(typeof(T) == typeof(GameObject))
                {
                    return obj as T;
                }
                T component = obj.GetComponent<T>();
                if (component == null)
                {
                    Debug.LogError($"Object{objectToSpawn.name} doesnt have the component of type: {typeof(T)}");
                }
                
                return component;
            }
            return null;
        }

        private static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, PoolType poolType = PoolType.GameObjects) where T : Component
        {
            return SpawnObject<T>(typePrefab.gameObject, spawnPos, Quaternion.identity, poolType);
        }
        
        public static GameObject SpawnGameObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRot, PoolType poolType = PoolType.GameObjects)
        {
            return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRot, poolType);
        }
        
        public static void ReturnToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
        {
            if(_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
            {
                GameObject parentObject = SetParentObject(poolType);
                
                if(obj.transform.parent != parentObject.transform)
                {
                    obj.transform.SetParent(parentObject.transform);
                }
                
                if(_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
                {
                    pool.Release(obj);
                }
            }
            else
            {
                Debug.LogWarning("Trying to return an object that is not in the pool: " + obj.name);
            }
        }
    }
}