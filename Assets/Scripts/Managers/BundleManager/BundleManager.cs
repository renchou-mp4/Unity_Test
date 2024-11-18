using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 已加载的资源信息
    /// </summary>
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

    /// <summary>
    /// 已加载的AB包信息
    /// </summary>
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
        /// <summary>
        /// 当前AB包已加载的资源信息
        /// </summary>
        public Dictionary<string, LoadedAssetInfo> _AssetDic
        {
            get => _assetDic;
            set => _assetDic = value;
        }
    }

    public class BundleManager : MonoSingleton<BundleManager>
    {
        /// <summary>
        /// Bundle文件夹在硬盘上的绝对路径
        /// </summary>
        public string _DiskBundlePath { get; private set; } = Application.streamingAssetsPath + "/Bundle";
        /// <summary>
        /// Bundle文件夹相对Assets的相对路径
        /// </summary>
        public string _RelativeBundlePath { get; private set; } = "Assets/Bundle";
        public string _ABExtension { get; } = ".ab";

        /// <summary>
        /// 已加载的AB包信息
        /// </summary>
        private Dictionary<string, LoadedBundleInfo> _loadedBundleDic = new();

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="bundlePath">从Bundle文件夹开始的AB包路径</param>
        /// <returns></returns>
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

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="bundleName">AB包名称</param>
        /// <param name="assetName">资源名称</param>
        /// <returns></returns>
        public T LoadAsset<T>(string bundleName, string assetName) where T : Object
        {
            LoadedBundleInfo bundleInfo = LoadBundle(GetDiskBundlePath(bundleName));
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

        /// <summary>
        /// 获取AB包在硬盘上的绝对路径
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private string GetDiskBundlePath(string bundleName)
        {
            return $"{_DiskBundlePath}/{bundleName}{_ABExtension}";
        }

        /// <summary>
        /// 获取资源的扩展名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetAssetExtension(System.Type type)
        {
            if (type == typeof(Sprite))
                return ".png";
            else
                return ".prefab";
        }
    }


}