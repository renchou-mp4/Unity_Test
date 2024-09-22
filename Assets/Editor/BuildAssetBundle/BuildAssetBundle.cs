using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;

public class BuildAssetBundle : IBuildBundle
{
    public void Build()
    {
        var bundleBuildParameters = new BundleBuildParameters(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildBundleTool._OutputPath);
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
        string[] allFilesPath = Directory.GetFiles(BuildBundleTool._BundlePath, BuildBundleTool.GetAllNeedBuildFileExtension(), SearchOption.AllDirectories);

        foreach (string filePath in allFilesPath)
        {
            allBuild.Add(new AssetBundleBuild
            {
                assetBundleName = FileTools.GetFileNameWithExtension(filePath),
                assetNames = new string[] { filePath }
            });
        }

        return allBuild.ToArray();
    }
}
