using System.Collections.Generic;
using System.IO;
using System.Linq;
using Managers;
using Tools;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace Editor.BuildAssetBundle
{
    /// <summary>
    ///     AB包信息类，用于输出Manifest文件
    /// </summary>
    public class BuildAssetBundle : IBuildBundle
    {
        /// <summary>
        ///     所有要打包的AssetBundleBuild
        /// </summary>
        private readonly HashSet<AssetBundleBuild> _allBuild = new();

        public void Build()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            CompatibilityAssetBundleManifest manifest = CompatibilityBuildPipeline.BuildAssetBundles(
                BuildBundleTools._OutputPath,
                GetAssetBundleBuild(),
                GetBuildAssetBundleOptions(),
                GetBuildTarget());

            File.WriteAllText(BuildBundleTools._ManifestOutputPath, JsonUtility.ToJson(manifest));
            File.Delete(BuildBundleTools._OutputPath + "/Bundle.manifest");
            File.Delete(BuildBundleTools._OutputPath + "/Bundle.manifest.meta");
            File.Delete(BuildBundleTools._OutputPath + "/buildlogtep.json");
            File.Delete(BuildBundleTools._OutputPath + "/buildlogtep.json.meta");

            AssetChangeTools.AssetChangeTools.UpdateAssetManifest();
        }

        /// <summary>
        ///     获取打包选项
        /// </summary>
        /// <returns></returns>
        private BuildAssetBundleOptions GetBuildAssetBundleOptions()
        {
            return BuildAssetBundleOptions.None;
        }

        /// <summary>
        ///     获取打包平台
        /// </summary>
        /// <returns></returns>
        private BuildTarget GetBuildTarget()
        {
            return BuildTarget.StandaloneWindows64;
        }

        /// <summary>
        ///     获取所有要构建的AssetBundleBuild信息
        /// </summary>
        /// <returns></returns>
        private AssetBundleBuild[] GetAssetBundleBuild()
        {
            string[] folderPaths = Directory.GetDirectories(BuildBundleTools._BundlePath, "*", SearchOption.AllDirectories);

            foreach (string folderPath in folderPaths)
            {
                string tmpFolderPath = folderPath.ReplacePathBackslash();
                if (BuildBundleTools.IsStartWithSpecifiedAssetPath(tmpFolderPath, BuildBundleTools.GetNeedBuildPathByDirectory()))
                    GetBundleBuild_Directory(tmpFolderPath);
                else
                    GetBundleBuild_Single(tmpFolderPath);
            }

            return _allBuild.ToArray();
        }

        /// <summary>
        ///     获取按文件夹打包的AssetBundleBuild信息
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        private void GetBundleBuild_Directory(string folderPath)
        {
            //处理文件
            string[] assetPaths = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly)
                .Where((filePath) => !filePath.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                .ToArray();

            try
            {
                if (assetPaths.Length <= 0) return;

                for (int i = 0; i < assetPaths.Count(); i++) assetPaths[i] = assetPaths[i].ReplacePathBackslash().RelativeToAssetPath();
                _allBuild.Add(new AssetBundleBuild
                {
                    assetBundleName = folderPath.RelativeToBundlePathWithoutBundle() + ExtensionTools.AB,
                    assetNames      = assetPaths
                });
            }
            catch
            {
                LogTools.Log($"存在相同BundleBuildName: 【{folderPath.RelativeToBundlePathWithoutBundle() + ExtensionTools.AB}】");
            }
        }

        /// <summary>
        ///     获取按单个文件打包的AssetBundleBuild信息
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        private void GetBundleBuild_Single(string folderPath)
        {
            //处理文件
            string[] assetPaths = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly)
                .Where(filePath => !filePath.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                .ToArray();

            foreach (string assetPath in assetPaths)
                try
                {
                    _allBuild.Add(new AssetBundleBuild
                    {
                        assetBundleName = assetPath.ReplacePathBackslashWithoutExtension().RelativeToBundlePathWithoutBundle() + ExtensionTools.AB,
                        assetNames      = new[] { assetPath.ReplacePathBackslash().RelativeToAssetPath() }
                    });
                }
                catch
                {
                    LogTools.Log($"存在相同BundleBuildName: 【{assetPath.ReplacePathBackslashWithoutExtension().RelativeToBundlePathWithoutBundle() + ExtensionTools.AB}】");
                }
        }
    }
}