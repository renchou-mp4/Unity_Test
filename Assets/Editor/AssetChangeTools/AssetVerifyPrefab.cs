using System.IO;
using Editor.BuildAssetBundle;
using GameDefine.DataDefine;
using Managers;
using Tools;
using UnityEditor;

namespace Editor.AssetChangeTools
{
    public class AssetVerifyPrefab : IAssetVerify
    {
        /// <summary>
        ///     该路径开头的资源忽略
        /// </summary>
        private readonly string[] _ignoredPath =
        {
            "Packages"
        };

        /// <summary>
        ///     Inspector面板显示的选项文本
        /// </summary>
        public string _WindowName => "Prefab检测";

        /// <summary>
        ///     是否启用该资源检测
        /// </summary>
        public bool _Enable { get; set; } = true;

        /// <summary>
        ///     资源导入检测
        /// </summary>
        /// <param name="assetPath"></param>
        public void AssetVerifyImport(string[] assetPath)
        {
            if (!_Enable) return;
            foreach (var path in assetPath)
            {
                if (!AssetTypeVerify(path))
                    continue;

                if (IsIgnoredPath(path))
                    continue;

                string assetName = Path.GetFileNameWithoutExtension(path);
                if (AssetChangeTools._AssetDic.TryAdd(assetName, new AssetManifestData(assetName, path.RelativeToBundlePathWithoutBundle())))
                    continue;

                LogTools.LogError($"添加资源路径失败！已有相同名字的资源：{assetName}，路径1：{path}，路径2：{AssetChangeTools._AssetDic[assetName]._bundleName}");
            }
        }

        /// <summary>
        ///     资源删除检测
        /// </summary>
        /// <param name="deletedAssets"></param>
        public void AssetVerifyDelete(string[] deletedAssets)
        {
            if (!_Enable) return;
            foreach (var assetPath in deletedAssets)
            {
                if (!AssetTypeVerify(assetPath))
                    continue;

                if (IsIgnoredPath(assetPath))
                    continue;

                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                if (AssetChangeTools._AssetDic.ContainsKey(assetName))
                {
                    AssetChangeTools._AssetDic.Remove(assetName);
                    continue;
                }

                LogTools.LogError($"删除资源路径失败！未找到对应资源：{assetName}");
            }
        }

        /// <summary>
        ///     资源移动检测
        /// </summary>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        public void AssetVerifyMove(string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!_Enable) return;
            int idx = -1;
            foreach (var assetPath in movedFromAssetPaths)
            {
                idx++;
                if (!AssetTypeVerify(assetPath))
                    continue;

                if (IsIgnoredPath(assetPath))
                    continue;

                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                if (AssetChangeTools._AssetDic.TryGetValue(assetName, out var assetManifestData))
                {
                    assetManifestData._bundleName = movedAssets[idx];
                    continue;
                }

                LogTools.LogError($"修改资源路径失败！未找到对应资源：{assetName}");
            }
        }

        /// <summary>
        ///     重置AssetManifest时处理该类型资源
        /// </summary>
        public void ResetAssetManifest()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (IsIgnoredPath(assetPath))
                    continue;
                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                if (assetName.IsNullOrEmpty() || assetPath.IsNullOrEmpty())
                {
                    LogTools.LogError($"重置AssetManifest时，添加Prefab失败！因为资源名称或路径为空！GUID：{guid}，资源名称：{assetName}");
                    break;
                }

                if (!AssetChangeTools._AssetDic.TryAdd(assetName, new AssetManifestData(assetName, assetPath.RelativeToBundlePathWithoutBundle())))
                {
                    LogTools.LogError($"重置AssetManifest时，添加{assetName}失败！路径：{assetPath}，类型：Prefab");
                    break;
                }
            }
        }

        /// <summary>
        ///     检测资源是否是该类型资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public bool AssetTypeVerify(string assetPath)
        {
            return assetPath.GetExtension() == ExtensionTools.PREFAB;
        }

        /// <summary>
        ///     是否忽略该路径
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private bool IsIgnoredPath(string assetPath)
        {
            return PathTools.IsStartWith(assetPath, _ignoredPath);
        }
    }
}