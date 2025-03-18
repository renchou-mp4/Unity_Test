using System;
using System.Collections.Generic;
using Managers;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using YooAsset;

namespace Tools
{
    public class GameObjectPool : SerializedMonoBehaviour,IDestroy
    {
        /// <summary>
        /// 对象池可配置参数
        /// </summary>
        [System.Serializable]
        public class PoolConfig
        {
            [Tooltip("要池化的预制体")]
            public GameObject _prefab;
            [Tooltip("对象池挂载的父节点")] 
            public Transform _poolRoot;
            [Tooltip("对象池最大数量，0表示无限")] 
            public int _poolMaxCount;
            [Tooltip("预加载数量")] 
            public int _poolPreloadCount;

            public PoolConfig(GameObject prefab, int poolMaxCount, int poolPreloadCount , Transform poolRoot = null)
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

        
        [Header("池配置")]
        public PoolConfig _config;
        [Tooltip("当前池中预制体总量"),ShowInInspector,ReadOnly]
        public int _PoolTotalCount{get; private set; }
        [Tooltip("当前池中已激活的预制体数量"),ShowInInspector,ReadOnly]
        public int _PoolCurCount{get; private set; }
        [Tooltip("已激活的对象池"),ShowInInspector,ReadOnly,OdinSerialize]
        private HashSet<GameObject> _activePool;
        [Tooltip("未激活的对象池"),ShowInInspector,ReadOnly,OdinSerialize]
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
            
            _activePool   = new HashSet<GameObject>(_config._poolMaxCount);
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

        public GameObject Spawn(Vector3 position, Quaternion rotation,Transform parent = null)
        {
            //从对象池中取
            _inactivePool.TryDequeue(out GameObject obj);
            
            //对象池无可用对象，创建新obj
            if (obj == null)
            {
                obj = CreateNewGameObject();
            }
            
            //初始化
            if (obj == null) return obj;
            
            obj.transform.SetParent(parent==null?_config._poolRoot:parent);
            obj.transform.SetPositionAndRotation(position, rotation);
            _activePool.Add(obj);
            _PoolCurCount++;
            if(!obj.activeSelf)
                obj.SetActive(true);
            return obj;
        }
        
        public GameObject Spawn(Transform parent = null)
        {
            return Spawn(Vector3.zero, Quaternion.identity,parent);
        }

        public void Despawn(GameObject obj)
        {
            if(obj==null)
                throw new NullReferenceException();
            
            _activePool.Remove(obj);
            _inactivePool.Enqueue(obj);
            _PoolCurCount--;
            obj.transform.SetParent(_config._poolRoot);
            if(obj.activeSelf)
                obj.SetActive(false);
        }

        private GameObject CreateNewGameObject()
        {
            if (_PoolTotalCount >= _config._poolMaxCount)
            {
                LogTools.LogError("已超出对象池最大数量限制！");
                return null;
            }
            
            GameObject newObj = Instantiate(_config._prefab, _config._poolRoot);
            _PoolTotalCount++;
            return newObj;
        }

        public void DespawnAll()
        {
            foreach (var obj in _activePool)
            {
                Despawn(obj);
            }
            _activePool.Clear();
            foreach (var obj in _inactivePool)
            {
                Destroy(obj);
            }
            _inactivePool.Clear();
            _PoolCurCount = _PoolTotalCount = 0;
        }
        
        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 20), 
                $"对象池：{_config._prefab.name}\n{_PoolCurCount} 激活 / {_PoolTotalCount - _PoolCurCount} 可用");
        }
        
        public void OnDestroy()
        {
            DespawnAll();
        }
        
        //TODO: 批处理和异步加载
        
    }
}
