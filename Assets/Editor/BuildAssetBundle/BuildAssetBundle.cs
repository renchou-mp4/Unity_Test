using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEngine;

/// <summary>
/// AB包信息类，用于输出Manifest文件
/// </summary>
[System.Serializable]
public class AssetBundleInfo
{
    public string _bundleName;
    public string[] _assetsName;
    public long _size;
    public string _md5;
}

/// <summary>
/// JsonUtility需要封装成类才可以转Json，不能直接转list
/// </summary>
[System.Serializable]
public class AssetBundleList
{
    public List<AssetBundleInfo> assetBundleInfos = new();

    public void AddData(AssetBundleInfo assetBundleInfo)
    {
        assetBundleInfos.Add(assetBundleInfo);
    }
}

public class BuildAssetBundle : IBuildBundle
{
    /// <summary>
    /// 所有要打包的AssetBundleBuild
    /// </summary>
    private HashSet<AssetBundleBuild> _allBuild = new();

    public void Build()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        var bundleBuildParameters = new BundleBuildParameters(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildBundleTools._OutputPath);
        var bundleBuildContent = new BundleBuildContent(GetAssetBundleBuild());
        IBundleBuildResults results;
        ReturnCode returnCode = ContentPipeline.BuildAssetBundles(bundleBuildParameters, bundleBuildContent, out results);
        if (returnCode == ReturnCode.Success)
        {
            LogTools.Log("打包完成！");
        }
        //删除多余Log文件
        File.Delete(BuildBundleTools._OutputPath + "/buildlogtep.json");
        File.Delete(BuildBundleTools._OutputPath + "/buildlogtep.json.meta");
        GenerateBundleManifest();
    }

    /// <summary>
    /// 获取所有要构建的AssetBundleBuild信息
    /// </summary>
    /// <returns></returns>
    private AssetBundleBuild[] GetAssetBundleBuild()
    {
        GetBundleBuild_Directory(BuildBundleTools._BundlePath);
        return _allBuild.ToArray();
    }

    /// <summary>
    /// 获取按文件夹打包的AssetBundleBuild信息
    /// </summary>
    /// <param name="path">从该路径开始获取</param>
    private void GetBundleBuild_Directory(string path)
    {
        //处理文件
        string[] allFilesPath = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        List<string> assetNames = new(allFilesPath.Count());

        foreach (string filePath in allFilesPath)
        {
            if (filePath.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                continue;
            assetNames.Add(filePath.ReplacePathBackslash().RelativeToAssetPath());
        }

        try
        {
            if (assetNames.Count > 0)
            {
                _allBuild.Add(new AssetBundleBuild
                {
                    assetBundleName = BuildBundleTools.GetBundleName(path) + BuildBundleTools._ABExtension,
                    assetNames = assetNames.ToArray(),
                });
            }
        }
        catch
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

    /// <summary>
    /// 获取按单个文件打包的AssetBundleBuild信息
    /// </summary>
    /// <param name="path">从该路径开始</param>
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
                    assetBundleName = BuildBundleTools.GetBundleName(filePath.ReplacePathBackslash()) + BuildBundleTools._ABExtension,
                    assetNames = new string[] { filePath.ReplacePathBackslash().RelativeToAssetPath() },
                });
            }
            catch
            {
                LogTools.Log($"存在相同BundleBuildName: 【{path + BuildBundleTools._ABExtension}】");
            }
        }


        //处理文件夹
        string[] allDirectoriesPath = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        foreach (string directoryPath in allDirectoriesPath)
        {
            string tmpPath = directoryPath.ReplacePathBackslash();
            GetBundleBuild_Single(tmpPath);
        }
    }

    /// <summary>
    /// 生成Manifest文件
    /// </summary>
    private void GenerateBundleManifest()
    {
        AssetBundleList assetBundleList = new();

        foreach (var build in _allBuild)
        {
            FileInfo fileInfo = new FileInfo(BuildBundleTools._OutputPath + "/" + build.assetBundleName);

            if (fileInfo.Name.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                continue;
            assetBundleList.AddData(new AssetBundleInfo
            {
                _bundleName = build.assetBundleName,
                _assetsName = build.assetNames,
                _size = fileInfo.Length,
                _md5 = EncryptionTools.EncryptionFileBySHA256(BuildBundleTools._OutputPath + "/" + build.assetBundleName)
            });
        }

        File.WriteAllText(BuildBundleTools._ManifestOutputPath, JsonUtility.ToJson(assetBundleList));
    }
}
