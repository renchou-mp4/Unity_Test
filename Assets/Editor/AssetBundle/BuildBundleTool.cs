using UnityEditor;

public class BuildBundleTool
{
    public enum BuildType
    {
        AssetBundle,
    }

    private static BuildType _curType = BuildType.AssetBundle;
    private static IBuildBundle _curBuilding = null;

    [MenuItem("BuildBundle/BuildAssetBundle")]
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
