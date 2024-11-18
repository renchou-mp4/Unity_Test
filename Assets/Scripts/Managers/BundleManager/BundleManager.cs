using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LoadedAssetInfo
    {
        public int _CiteCount;
        private Object _asset;
        public Object _Asset
        {
            get
            {
                _CiteCount++;
                return _asset;
            }
            set => _asset = value;
        }
    }

    public class LoadedBundleInfo
    {
        public int _CiteCount;
        private AssetBundle _assetBundle;
        public AssetBundle _AssetBundle
        {
            get
            {
                _CiteCount++;
                return _assetBundle;
            }
            set => _assetBundle = value;
        }
        private Dictionary<string, LoadedAssetInfo> _assetDic = new();
        public Dictionary<string, LoadedAssetInfo> _AssetDic
        {
            get => _assetDic;
            set => _assetDic = value;
        }
    }

    public class BundleManager : MonoSingleton<BundleManager>
    {
        public string _DiskBundlePath { get; private set; } = Application.streamingAssetsPath + "/Bundle";
        public string _RelativeBundlePath { get; private set; } = "Assets/Bundle";
        public string _ABExtension { get; } = ".ab";

        private Dictionary<string, LoadedBundleInfo> _loadedBundleDic = new();

        public LoadedBundleInfo LoadBundle(string bundlePath)
        {
            if (!_loadedBundleDic.ContainsKey(bundlePath))
            {
                _loadedBundleDic.Add(bundlePath, new LoadedBundleInfo()
                {
                    _CiteCount = 0,
                    _AssetBundle = AssetBundle.LoadFromFile(bundlePath),
                });
            }
            return _loadedBundleDic[bundlePath];
        }

        public T LoadAsset<T>(string bundleName, string assetName) where T : Object
        {
            LoadedBundleInfo bundleInfo = LoadBundle(GetDiskBundlePath<T>(bundleName));
            if (!bundleInfo._AssetDic.ContainsKey(assetName))
            {
                string assetFullName = $"{_RelativeBundlePath}/{bundleName}/{assetName}{GetAssetExtension(typeof(T))}";
                bundleInfo._AssetDic.Add(assetName, new LoadedAssetInfo
                {
                    _CiteCount = 0,
                    _Asset = bundleInfo._AssetBundle.LoadAsset<T>(assetFullName),
                });
            }
            return bundleInfo._AssetDic[assetName]._Asset as T;
        }

        private string GetDiskBundlePath<T>(string bundleName)
        {
            return $"{_DiskBundlePath}/{bundleName}{_ABExtension}";
        }

        private string GetAssetExtension(System.Type type)
        {
            if (type == typeof(Sprite))
                return ".png";
            else
                return ".prefab";
        }
    }


}