using System.Collections.Generic;
using System.IO;
using Tools;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace Managers
{
    public class BundleManager : MonoSingleton<BundleManager>
    {
        /// <summary>
        /// Bundle文件夹在硬盘上的绝对路径
        /// </summary>
        private string _DiskBundlePath { get; } = Application.streamingAssetsPath + "/Bundle";

        /// <summary>
        /// Bundle文件夹相对Assets的相对路径
        /// </summary>
        private const string RELATIVE_BUNDLE_PATH = "Assets/Bundle";

        /// <summary>
        /// 已加载的AB包信息
        /// </summary>
        private readonly Dictionary<string, LoadedBundleInfo> _loadedBundleDic = new();

        private CompatibilityAssetBundleManifest _manifest;
        private CompatibilityAssetBundleManifest _Manifest { get; set; }

        protected override void SingletonInit()
        {
            base.SingletonInit();
            InitManifest();
        }

        private void InitManifest()
        {
            if (_Manifest != null) return;
            
            string manifest = File.ReadAllText(Application.dataPath + "/asset_version.json");
            _Manifest = ScriptableObject.CreateInstance<CompatibilityAssetBundleManifest>();
            JsonUtility.FromJsonOverwrite(manifest, _Manifest);
        }

        /// <summary>
        /// 使用包名加载AB包
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        /// <param name="isDepend">是否为依赖包</param>
        /// <returns></returns>
        public LoadedBundleInfo LoadBundle(string bundleName,bool isDepend = false)
        {
            if (!bundleName.Contains(ExtensionTools.AB))
                bundleName += ExtensionTools.AB;

            if (_loadedBundleDic.TryGetValue(bundleName, out var bundle)) 
                return bundle;
            
            if(!isDepend)
                LoadDependencyBundle(bundleName);
            
            _loadedBundleDic.Add(bundleName, new LoadedBundleInfo()
            {
                _RefCount   = 0,
                _AssetBundle = AssetBundle.LoadFromFile(GetDiskBundlePath(bundleName)),
            });

            return _loadedBundleDic[bundleName];
        }

        /// <summary>
        /// 使用AB包名加载它的依赖包
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        private void LoadDependencyBundle(string bundleName)
        {
            //获取所有依赖包，包括依赖包的依赖包
            string[] dependencyBundleNames = _Manifest.GetAllDependencies(bundleName);
            
            foreach (string dependencyBundleName in dependencyBundleNames)
            {
                if (!_loadedBundleDic.ContainsKey(dependencyBundleName))
                {
                    LoadBundle(dependencyBundleName,true);
                }
            }
        }

        /// <summary>
        /// 使用资源名加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="bundleName">AB包名称</param>
        /// <param name="assetName">资源名称</param>
        /// <returns></returns>
        public T LoadAsset<T>(string bundleName, string assetName) where T : Object
        {
            LoadedBundleInfo bundleInfo = LoadBundle(bundleName);
            
            if (bundleInfo._AssetDic.TryGetValue(assetName, out var assetInfo)) 
                return assetInfo._Asset as T;
            
            string assetFullName = GetAssetFullName<T>(bundleName, assetName);
            bundleInfo._AssetDic.Add(assetName, new LoadedAssetInfo
            {
                _RefCount = 0,
                _Asset     = bundleInfo._AssetBundle.LoadAsset<T>(assetFullName),
            });

            return bundleInfo._AssetDic[assetName]._Asset as T;
        }

        /// <summary>
        /// 获取AB包在硬盘上的绝对路径
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private string GetDiskBundlePath(string bundleName)
        {
            return $"{_DiskBundlePath}/{bundleName}";
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
            return ".prefab";
        }

        /// <summary>
        /// 获取资源全名
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        /// <param name="assetName">资源名</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        private string GetAssetFullName<T>(string bundleName, string assetName)
        {
            return IsSingleAssetBundle(bundleName, assetName) ?
                //单文件AB
                $"{RELATIVE_BUNDLE_PATH}/{bundleName}{GetAssetExtension(typeof(T))}" :
                //按文件夹打包AB
                $"{RELATIVE_BUNDLE_PATH}/{bundleName}/{assetName}{GetAssetExtension(typeof(T))}";
        }

        /// <summary>
        /// 检测是否是单文件AB
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        /// <param name="assetName">资源名</param>
        /// <returns></returns>
        private bool IsSingleAssetBundle(string bundleName, string assetName)
        {
            return bundleName.Contains(assetName);
        }
    }
}