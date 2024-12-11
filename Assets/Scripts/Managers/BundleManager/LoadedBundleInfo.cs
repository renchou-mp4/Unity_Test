using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 已加载的AB包信息
    /// </summary>
    public class LoadedBundleInfo
    {
        public int _RefCount;
        private AssetBundle _assetBundle;

        public AssetBundle _AssetBundle
        {
            get
            {
                _RefCount++;
                return _assetBundle;
            }
            set => _assetBundle = value;
        }

        /// <summary>
        /// 当前AB包已加载的资源信息
        /// </summary>
        public Dictionary<string, LoadedAssetInfo> _AssetDic { get; } = new();
    }
}