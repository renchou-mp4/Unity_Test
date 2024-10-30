using Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;

public class BuildAssetBundle : IBuildBundle
{
    private HashSet<AssetBundleBuild> _allBuild = new();

    public void Build()
    {
        var bundleBuildParameters = new BundleBuildParameters(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildBundleTools._OutputPath);
        var bundleBuildContent = new BundleBuildContent(GetAssetBundleBuild());
        IBundleBuildResults results;
        ContentPipeline.BuildAssetBundles(bundleBuildParameters, bundleBuildContent, out results);
    }

    private AssetBundleBuild[] GetAssetBundleBuild()
    {
        GetBundleBuild_Directory(BuildBundleTools._BundlePath);


        //获取指定文件夹下所有的资源路径
        //string[] allFilesPath = Directory.GetFiles(BuildBundleTools._BundlePath, "*", SearchOption.AllDirectories);

        //foreach (string filePath in allFilesPath)
        //{
        //    if (filePath.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
        //        continue;

        //    //按目录打包的资源
        //    string filetmpPath = filePath.ReplacePathBackslash();
        //    if (BuildBundleTools.IsStartWithSpecifiedAssetPath(filetmpPath))
        //    {
        //        GetBundleBuild_Directory(filetmpPath);
        //    }

        //}

        return _allBuild.ToArray();
    }


    private void GetBundleBuild_Directory(string path)
    {
        //处理文件
        string[] allFilesPath = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        List<string> assetNames = new(allFilesPath.Count());
        foreach (string filePath in allFilesPath)
        {
            if (filePath.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                continue;
            assetNames.Add(BuildBundleTools.GetAssetPath(filePath.ReplacePathBackslash()));
        }

        try
        {
            if (assetNames.Count > 0)
            {
                _allBuild.Add(new AssetBundleBuild
                {
                    assetBundleName = path.GetDirectoryName() + BuildBundleTools._ABExtension,
                    assetNames = assetNames.ToArray(),
                });
            }
        }
        catch (Exception e)
        {
            LogManager.Log("存在相同BundleBuildName！" + e.Message);
        }

        //处理文件夹
        string[] allDirectoriesPath = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        foreach (string directoryPath in allDirectoriesPath)
        {
            string tmpPath = directoryPath.ReplacePathBackslash();
            if (BuildBundleTools.IsStartWithSpecifiedAssetPath(tmpPath, BuildBundleTools.GetNeedBuildPathByDirectory()))
            {
                GetBundleBuild_Directory(tmpPath);
            }
        }
    }
}
