using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class LoadedBundleInfo
    {
        public int _CiteCount;
        private AssetBundle _assetBundle;
        public AssetBundle _AssetBundle
        {
            get
            {
                _CiteCount++;
                return _AssetBundle;
            }
            set => _assetBundle = value;
        }
    }

    public class BundleManager : MonoSingleton<BundleManager>
    {
        private static Dictionary<string, LoadedBundleInfo> _loadedBundle = new();

        public static AssetBundle LoadBundle(string filePath)
        {
            if (!_loadedBundle.ContainsKey(filePath))
            {
                _loadedBundle.Add(filePath, new LoadedBundleInfo()
                {
                    _CiteCount = 0,
                    _AssetBundle = AssetBundle.LoadFromFile(filePath),
                });
            }
            return _loadedBundle[filePath]._AssetBundle;
        }
    }
}