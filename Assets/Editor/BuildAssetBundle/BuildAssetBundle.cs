using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Tools;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;

public class AssetBundleInfo
{
    public string _BundleName { get; set; }
    public string[] _AssetsName { get; set; }
    public long _Size { get; set; }
    public SHA256 _MD5 { get; set; }
}


public class BuildAssetBundle : IBuildBundle
{
    private HashSet<AssetBundleBuild> _allBuild = new();

    public void Build()
    {
        var bundleBuildParameters = new BundleBuildParameters(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildBundleTools._OutputPath);
        var bundleBuildContent = new BundleBuildContent(GetAssetBundleBuild());
        IBundleBuildResults results;
        ReturnCode returnCode = ContentPipeline.BuildAssetBundles(bundleBuildParameters, bundleBuildContent, out results);
        if (returnCode == ReturnCode.Success)
        {
            LogTools.Log("打包完成！");
        }
    }

    private AssetBundleBuild[] GetAssetBundleBuild()
    {
        GetBundleBuild_Directory(BuildBundleTools._BundlePath);
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
            LogTools.Log($"存在相同BundleBuildName: 【{path + BuildBundleTools._ABExtension}】");
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
            else
            {
                //默认使用单个文件构建AB包
                GetBundleBuild_Single(tmpPath);
            }
        }
    }

    private void GetBundleBuild_Single(string path)
    {
        //处理文件
        string[] allFilesPath = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        foreach (string filePath in allFilesPath)
        {
            if (filePath.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                continue;
            try
            {
                _allBuild.Add(new AssetBundleBuild
                {
                    assetBundleName = path + BuildBundleTools._ABExtension,
                    assetNames = new string[] { BuildBundleTools.GetAssetPath(filePath.ReplacePathBackslash()) },
                });
            }
            catch (Exception e)
            {
                LogTools.Log($"存在相同BundleBuildName: 【{path + BuildBundleTools._ABExtension}】");
            }
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
            else
            {
                //默认使用单个文件构建AB包
                GetBundleBuild_Single(tmpPath);
            }
        }
    }

    private void GenerateBundleManifest()
    {
        List<AssetBundleInfo> assetBundleInfos = new();
        //获取所有AB包文件
        FileInfo[] fileInfos = new DirectoryInfo(BuildBundleTools._OutputPath).GetFiles();

        HashSet<AssetBundleBuild>.Enumerator enumerator = _allBuild.GetEnumerator();
        for (int i = 0; i < fileInfos.Length; i++, enumerator.MoveNext())
        {
            assetBundleInfos.Add(new AssetBundleInfo
            {
                _BundleName = enumerator.Current.assetBundleName,
                _AssetsName = enumerator.Current.assetNames,
                _Size = fileInfos[i].Length,
                _MD5 = SHA256.Create()
            });
        }

        //File.WriteAllText(BuildBundleTools._ManifestOutputPath,);
    }
}
