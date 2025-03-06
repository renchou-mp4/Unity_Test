using System;
using System.Collections.Generic;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;

namespace Tools
{
    public class GameObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class PoolConfig
        {
            [Tooltip("要池化的预制体")]
            public GameObject _prefab;
            [Tooltip("对象池挂载的父节点")] 
            public Transform _poolRoot;
            [Tooltip("对象池最大数量，0表示无限")]
            public int        _poolMaxCount;
            [Tooltip("预加载数量")]
            public int        _poolPreloadCount;

            public PoolConfig(GameObject prefab, int poolMaxCount, int poolPreloadCount,Transform poolRoot = null)
            {
                _prefab = prefab;
                _poolRoot = poolRoot;
                _poolMaxCount = poolMaxCount;
                _poolPreloadCount = poolPreloadCount;

                if (_prefab == null)
                {
                    throw new NullReferenceException();
                }
                if (_poolPreloadCount < 0)
                {
                    LogTools.LogError("预加载数量小于0！");
                    _poolPreloadCount = 0;
                }
                if (_poolMaxCount != 0 && _poolPreloadCount > _poolMaxCount)
                {
                    LogTools.LogError("预加载数量大于池最大数量！");
                    _poolMaxCount = 0;
                }
            }
        }

        [Tooltip("当前池中已激活的预制体数量"),ShowInInspector]
        public int _PoolCurCount{get; private set; }
        [Tooltip("当前池中预制体总量"),ShowInInspector]
        public int _PoolTotalCount{get; private set; }
        [Header("池配置")]
        public PoolConfig _config;

        private Queue<GameObject> _activePool;
        private Queue<GameObject> _inactivePool;

        public GameObjectPool(PoolConfig config)
        {
            _config = config;
            PreloadPool();
        }

        private void PreloadPool()
        {
            
            //设置父节点
            if (_config._poolRoot == null)
            {
                GameObject root = new GameObject($"Pool_{_config._prefab.name}");
                root.transform.SetParent(transform);
                _config._poolRoot = root.transform;
            }
            
            _inactivePool = new Queue<GameObject>(_config._poolMaxCount);
            
            //预加载GameObject
            for (int i = 0; i < _config._poolPreloadCount; i++)
            {
                GameObject obj = Instantiate(_config._prefab,_config._poolRoot);
                _inactivePool.Enqueue(obj);
                if(obj.activeSelf)
                    obj.SetActive(false);
            }
            
            _PoolCurCount = _config._poolPreloadCount;
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            if (_inactivePool.TryDequeue(out GameObject obj))
            {
                obj.transform.SetPositionAndRotation(position, rotation);
                if (!obj.activeSelf)
                    obj.SetActive(true);
                _activePool.Enqueue(obj);
                _PoolCurCount++;
            }

            GameObject newObj = CreateNewGameObject();
            newObj?.transform.SetPositionAndRotation(position, rotation);
            return newObj;
        }

        public void Despawn(GameObject obj)
        {
            
        }

        private GameObject CreateNewGameObject()
        {
            if (_PoolTotalCount >= _config._poolMaxCount)
            {
                LogTools.LogError("已超出对象池最大数量限制！");
                return null;
            }
            
            GameObject newObj = Instantiate(_config._prefab, _config._poolRoot);
            _activePool.Enqueue(newObj);
            _PoolCurCount++;
            return newObj;
        }
    }
}
