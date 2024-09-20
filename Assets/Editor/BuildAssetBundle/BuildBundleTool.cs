using UnityEngine;

public class BuildBundleTool
{
    public enum BuildType
    {
        AssetBundle,
    }

    public static string _OutputPath { get; private set; } = "D:/";
    public static string _BundlePath { get; private set; } = Application.dataPath + "/Bundle";


    private static BuildType _curType = BuildType.AssetBundle;
    private static IBuildBundle _curBuilding = null;


    public static void BuildBundle()
    {
        switch (_curType)
        {
            case BuildType.AssetBundle:
                _curBuilding = new BuildAssetBundle();
                _curBuilding.Build();
                break;
        }
    }
}
