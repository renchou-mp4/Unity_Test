using System.Collections.Generic;
using System.IO;
using Tools;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;

public class BuildAssetBundle : IBuildBundle
{
    public void Build()
    {
        var bundleBuildParameters = new BundleBuildParameters(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildBundleTools._OutputPath);
        var bundleBuildContent = new BundleBuildContent(GetAssetBundleBuild());
        IBundleBuildResults results;
        ContentPipeline.BuildAssetBundles(bundleBuildParameters, bundleBuildContent, out results);
    }

    private AssetBundleBuild[] GetAssetBundleBuild()
    {
        AssetBundleBuild[] assetBundleBuild = GetBundleBuild_AllAssetSingle();

        return assetBundleBuild;
    }

    private AssetBundleBuild[] GetBundleBuild_AllAssetSingle()
    {
        List<AssetBundleBuild> allBuild = new();

        //获取指定文件夹下所有的资源路径
        string[] allFilesPath = Directory.GetFiles(BuildBundleTools._BundlePath, "*", SearchOption.AllDirectories);

        foreach (string filePath in allFilesPath)
        {
            if (!filePath.IsEndWith(BuildBundleTools.GetAllNeedBuildFileExtension()))
                continue;

            string fileTmpPath = filePath.ReplacePathBackslash();
            allBuild.Add(new AssetBundleBuild
            {
                assetBundleName = fileTmpPath.GetFileNameWithExtension() + BuildBundleTools._ABExtension,
                assetNames = new string[] { fileTmpPath.RelativeToAssetPath() }
            });
        }

        return allBuild.ToArray();
    }
}
