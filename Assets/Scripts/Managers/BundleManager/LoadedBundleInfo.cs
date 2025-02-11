using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    /// <summary>
    ///     已加载的AB包信息
    /// </summary>
    [Serializable]
    public class LoadedBundleInfo
    {
        [SerializeField] private int _refCount;

        private AssetBundle _assetBundle;

        public AssetBundle _AssetBundle { get; set; }
        
        public int _RefCount
        {
            get => _refCount;
            set
            {
                _refCount = value;
                if (_refCount <= 0)
                    UnloadBundle();
            }
        }
        
        /// <summary>
        ///     当前AB包已加载的资源信息
        /// </summary>
        [ShowInInspector, Title("已加载的资源"), DictionaryDrawerSettings(KeyLabel = "资源名", ValueLabel = "资源信息")]
        public Dictionary<string, LoadedAssetInfo> _AssetDic { get; } = new();

        private void UnloadBundle()
        {
            if (_assetBundle is null)
                LogTools.LogError($"无法卸载该AB包！【{nameof(_assetBundle)}】 是空！");
            else
                _assetBundle.Unload(true);
        }
    }
}