using UnityEditor;
using UnityEditor.Build.Content;
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
        AssetBundleBuild[] assetBundleBuild = ContentBuildInterface.GenerateAssetBundleBuilds();

        return assetBundleBuild;
    }

    private void GetBundleBuild_AllAssetSingle()
    {
        //获取指定文件夹下所有的资源
    }
}
