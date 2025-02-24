using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameDefine.DataDefine;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Tools;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace Managers
{
    public class BundleManager : BaseManager<BundleManager>
    {
        /// <summary>
        ///     Bundle文件夹相对Assets的相对路径
        /// </summary>
        private const string RELATIVE_BUNDLE_PATH = "Assets/Bundle";

        /// <summary>
        ///     已加载的AB包信息--AB包名，AB包信息
        /// </summary>
        [ShowInInspector, Title("已加载的AB包"), DictionaryDrawerSettings(KeyLabel = "包名", ValueLabel = "AB包信息")]
        private readonly Dictionary<string, LoadedBundleInfo> _loadedBundleDic = new();

        /// <summary>
        ///     已加载的依赖包--AB包名，依赖包名
        /// </summary>
        private readonly Dictionary<string, List<LoadedBundleInfo>> _loadedDependencyBundleDic = new();

        private CompatibilityAssetBundleManifest _manifest;

        /// <summary>
        ///     资源字典，资源名和资源路径的对应《资源名称，资源信息》
        /// </summary>
        private Dictionary<string, AssetManifestData> _AssetDic { get; set; } = new();

        /// <summary>
        ///     Bundle文件夹在硬盘上的绝对路径
        /// </summary>
        private string _DiskBundlePath { get; } = Application.streamingAssetsPath + "/Bundle";

        private CompatibilityAssetBundleManifest _Manifest { get; set; }

        public void Start()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using StreamReader reader = new StreamReader(PathTools._AssetManifestPath);

            string context = reader.ReadToEnd();
            _AssetDic = JsonConvert.DeserializeObject<Dictionary<string, AssetManifestData>>(context);

            stopwatch.Stop();
            LogTools.Log($"加载prefab列表完成，用时：【{stopwatch.ElapsedMilliseconds}】毫秒");
        }

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
        ///     使用包名加载AB包
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        /// <param name="isDepend">是否为依赖包</param>
        /// <returns></returns>
        public LoadedBundleInfo LoadBundle(string bundleName, bool isDepend = false)
        {
            if (!bundleName.EndsWith(ExtensionTools.AB))
                bundleName += ExtensionTools.AB;

            if (_loadedBundleDic.TryGetValue(bundleName, out var bundle))
            {
                bundle._RefCount++;
                return bundle;
            }

            if (!isDepend)
                LoadDependencyBundle(bundleName);

            _loadedBundleDic.Add(bundleName, new LoadedBundleInfo()
            {
                _RefCount    = 1,
                _AssetBundle = AssetBundle.LoadFromFile(GetDiskBundlePath(bundleName))
            });

            return _loadedBundleDic[bundleName];
        }

        /// <summary>
        ///     使用AB包名加载它的依赖包
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        private void LoadDependencyBundle(string bundleName)
        {
            //获取所有依赖包，包括依赖包的依赖包
            string[] dependencyBundleNames = _Manifest.GetAllDependencies(bundleName);

            foreach (string dependencyBundleName in dependencyBundleNames)
            {
                if (_loadedBundleDic.TryGetValue(dependencyBundleName, out var dependencyBundle))
                    dependencyBundle._RefCount++;
                else
                    dependencyBundle = LoadBundle(dependencyBundleName, true);

                if (_loadedDependencyBundleDic.TryGetValue(bundleName, out var dependencyBundles))
                    dependencyBundles.Add(dependencyBundle);
                else
                    _loadedDependencyBundleDic.Add(bundleName, new List<LoadedBundleInfo>() { dependencyBundle });
            }
        }

        /// <summary>
        ///     卸载AB包
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        public void UnloadBundle(string bundleName)
        {
            if (!bundleName.EndsWith(ExtensionTools.AB))
                bundleName += ExtensionTools.AB;

            if (!_loadedBundleDic.TryGetValue(bundleName, out var bundle))
            {
                LogTools.LogWarning($"该AB包未加载，无法卸载！--【{bundleName}】");
                return;
            }

            bundle._RefCount--;
            if (bundle._RefCount != 0) return;

            _loadedBundleDic.Remove(bundleName);
            UnloadDependencyBundle(bundleName);
        }

        /// <summary>
        ///     卸载指定AB包的依赖包
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        private void UnloadDependencyBundle(string bundleName)
        {
            if (!_loadedDependencyBundleDic.TryGetValue(bundleName, out var bundles))
                return;

            //卸载依赖包
            foreach (var bundle in bundles) UnloadBundle(bundle._AssetBundle.name);

            _loadedDependencyBundleDic.Remove(bundleName);
        }


        /// <summary>
        ///     使用资源名加载资源，单个资源打AB的才可以缺省bundleName
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="bundleName">AB包名称</param>
        /// <param name="assetName">资源名称</param>
        /// <returns></returns>
        public T LoadAsset<T>(string assetName, string bundleName = null) where T : UnityEngine.Object
        {
            bundleName ??= GetBundleNameByAssetName(assetName);
            LoadedBundleInfo bundleInfo = LoadBundle(bundleName);

            if (bundleInfo._AssetDic.TryGetValue(assetName, out var assetInfo))
                return assetInfo._Asset as T;

            string assetFullName = GetAssetFullName<T>(bundleName, assetName);
            bundleInfo._AssetDic.Add(assetName, new LoadedAssetInfo
            {
                _refCount = 1,
                _Asset    = bundleInfo._AssetBundle.LoadAsset<T>(assetFullName)
            });

            return bundleInfo._AssetDic[assetName]._Asset as T;
        }

        /// <summary>
        ///     获取AB包在硬盘上的绝对路径
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private string GetDiskBundlePath(string bundleName)
        {
            return $"{_DiskBundlePath}/{bundleName}";
        }

        private string GetBundleNameByAssetName(string assetName)
        {
            return _AssetDic.TryGetValue(assetName, out var assetManifestData) ? assetManifestData._bundleName : string.Empty;
        }

        /// <summary>
        ///     获取资源的扩展名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetAssetExtension(Type type)
        {
            if (type == typeof(Sprite))
                return ".png";
            return ".prefab";
        }

        /// <summary>
        ///     获取资源全名
        /// </summary>
        /// <param name="bundleName">AB包名</param>
        /// <param name="assetName">资源名</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        private string GetAssetFullName<T>(string bundleName, string assetName)
        {
            return IsSingleAssetBundle(bundleName, assetName)
                ?
                //单文件AB
                $"{RELATIVE_BUNDLE_PATH}/{bundleName}{GetAssetExtension(typeof(T))}"
                :
                //按文件夹打包AB
                $"{RELATIVE_BUNDLE_PATH}/{bundleName}/{assetName}{GetAssetExtension(typeof(T))}";
        }

        /// <summary>
        ///     检测是否是单文件AB
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