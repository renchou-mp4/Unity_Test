using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEngine;


[System.Serializable]
public class AssetBundleInfo
{
    public string _bundleName;
    public string[] _assetsName;
    public long _size;
    public string _md5;
}

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
    private HashSet<AssetBundleBuild> _allBuild = new();

    public void Build()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        var bundleBuildParameters = new BundleBuildParameters(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildBundleTools._OutputPath);
        var bundleBuildContent = new BundleBuildContent(GetAssetBundleBuild());
        IBundleBuildResults results;
        ReturnCode returnCode = ContentPipeline.BuildAssetBundles(bundleBuildParameters, bundleBuildContent, out results);
        if (returnCode == ReturnCode.Success)
        {
            LogTools.Log("打包完成！");
        }
        File.Delete(BuildBundleTools._OutputPath + "/buildlogtep.json");
        File.Delete(BuildBundleTools._OutputPath + "/buildlogtep.json.meta");
        GenerateBundleManifest();
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
                    assetBundleName = filePath.ReplacePathBackslash().GetFileNameWithExtension() + BuildBundleTools._ABExtension,
                    assetNames = new string[] { BuildBundleTools.GetAssetPath(filePath.ReplacePathBackslash()) },
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
        AssetBundleList assetBundleList = new();

        //获取所有AB包文件
        FileInfo[] fileInfos = new DirectoryInfo(BuildBundleTools._OutputPath).GetFiles();

        HashSet<AssetBundleBuild>.Enumerator enumerator = _allBuild.GetEnumerator();
        enumerator.MoveNext();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Name.IsEndWith(BuildBundleTools.GetNoNeedBuildFileExtension()))
                continue;
            assetBundleList.AddData(new AssetBundleInfo
            {
                _bundleName = enumerator.Current.assetBundleName,
                _assetsName = enumerator.Current.assetNames,
                _size = fileInfos[i].Length,
                _md5 = EncryptionTools.EncryptionFileBySHA256(BuildBundleTools._OutputPath + "/" + enumerator.Current.assetBundleName)
            });
            enumerator.MoveNext();
        }
        File.WriteAllText(BuildBundleTools._ManifestOutputPath, JsonUtility.ToJson(assetBundleList));
    }
}
