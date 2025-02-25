#region

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Tools;
using UnityEngine;
using YooAsset;

#endregion

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

        public ResourcePackage _Package { get; set; }

        public IEnumerator InitResourceList()
        {
            yield return null;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using var reader = new StreamReader(PathTools._AssetManifestPath);

            var context = reader.ReadToEnd();
            _AssetDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(context);

            stopwatch.Stop();
            LogTools.Log($"加载prefab列表完成，用时：【{stopwatch.ElapsedMilliseconds}】毫秒");
            yield return null;
        }

        public IEnumerator InitYooAssets()
        {
            //初始化资源系统
            YooAssets.Initialize();

            //设置默认Package
            _Package = YooAssets.CreatePackage("TestPackage");
            YooAssets.SetDefaultPackage(_Package);

            //初始化Package
            yield return StartCoroutine(InitPackageOffline());
            //初始化Package版本信息
            yield return StartCoroutine(InitPackageVersion());
            //初始化资源列表
            yield return StartCoroutine(InitResourceList());

            
            //
            // //更新资源清单
            // var operation2 = _Package.UpdatePackageManifestAsync(packageVersion);
            // yield return operation2;
            //
            // if (operation2.Status == EOperationStatus.Succeed)
            //     //更新成功
            //     LogTools.Log("资源更新成功！");
            // else
            //     //更新失败
            //     LogTools.LogError(operation2.Error);

            
        }

        private IEnumerator InitPackageOffline()
        {
            //初始化本地Package
            // ReSharper disable once IdentifierTypo
            var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            var initParameters = new OfflinePlayModeParameters
            {
                BuildinFileSystemParameters = buildinFileSystemParams
            };
            var initOperation = _Package.InitializeAsync(initParameters);
            yield return initOperation;

            if (initOperation.Status == EOperationStatus.Succeed)
                LogTools.Log("资源包初始化成功！");
            else
                LogTools.LogError($"资源包初始化失败：{initOperation.Error}");
        }

        private IEnumerator InitPackageVersion()
        {
            //更新资源版本
            var request = _Package.RequestPackageVersionAsync();
            yield return request;

            if (request.Status == EOperationStatus.Succeed)
            {
                //更新成功
                var packageVersion = request.PackageVersion;
                LogTools.Log($"请求Package版本成功！【{packageVersion}】");
            }
            else
            {
                //更新失败
                LogTools.LogError($"请求Package版本失败！{request.Error}");
            }
        }

        public AssetHandle LoadAssetAsync<T>(string assetName) where T : Object
        {
            if (_AssetDic.TryGetValue(assetName, out var value))
            {
                var handle = _Package.LoadAssetAsync<T>(value);
                return handle;
            }

            LogTools.LogError($"Manifest中不包含【{assetName}】资源！");
            return null;
        }
    }
}