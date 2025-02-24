using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Tools;
using UnityEditor;
using YooAsset;

namespace Managers
{
    public enum PlayMode
    {
        EditorSimulateMode,
        OfflinePlayMode,
        HostPlayMode,
        WebPlayMode
    }

    public class BundleManagerYooAsset : MonoSingleton<BundleManagerYooAsset>
    {
        /// <summary>
        ///     资源字典，资源名和资源路径的对应《资源名称，资源信息》
        /// </summary>
        private static Dictionary<string, string> _AssetDic { get; set; } = new();
        
        public ResourcePackage _package;
        
        [InitializeOnLoadMethod]
        public static void Init()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using StreamReader reader = new StreamReader(PathTools._AssetManifestPath);

            string context = reader.ReadToEnd();
            _AssetDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(context);

            stopwatch.Stop();
            LogTools.Log($"加载prefab列表完成，用时：【{stopwatch.ElapsedMilliseconds}】毫秒");
        }
        
        public IEnumerator InitPackage()
        {
            //初始化资源系统
            YooAssets.Initialize();
            
            //设置默认Package
            _package = YooAssets.CreatePackage("TestPackage");
            YooAssets.SetDefaultPackage(_package);
            
            //初始化本地Package
            // ReSharper disable once IdentifierTypo
            var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            var initParameters          = new OfflinePlayModeParameters
            {
                BuildinFileSystemParameters = buildinFileSystemParams
            };
            var initOperation = _package.InitializeAsync(initParameters);
            yield return initOperation;
    
            if(initOperation.Status == EOperationStatus.Succeed)
                LogTools.Log("资源包初始化成功！");
            else 
                LogTools.LogError($"资源包初始化失败：{initOperation.Error}");
            
            //更新资源版本
            var operation1 = _package.RequestPackageVersionAsync();
            yield return operation1;

            string packageVersion = "";
            if (operation1.Status == EOperationStatus.Succeed)
            {
                //更新成功
                packageVersion = operation1.PackageVersion;
                LogTools.Log($"Request package Version : {packageVersion}");
            }
            else
            {
                //更新失败
                LogTools.LogError(operation1.Error);
            }
            
            //更新资源清单
            var operation2 = _package.UpdatePackageManifestAsync(packageVersion);
            yield return operation2;

            if (operation2.Status == EOperationStatus.Succeed)
            {
                //更新成功
                LogTools.Log("资源更新成功！");
            }
            else
            {
                //更新失败
                LogTools.LogError(operation2.Error);
            }
        }

        public AssetHandle LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
            if (_AssetDic.TryGetValue(assetName, out var value))
            {
                AssetHandle handle = _package.LoadAssetAsync<T>(value); 
                return handle; 
            }
            LogTools.LogError($"Manifest中不包含【{assetName}】资源！");
            return null;
        }
    }
}