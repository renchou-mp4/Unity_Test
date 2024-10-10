using UnityEngine;

namespace Tools
{
    public static class BuildBundleTools
    {
        public enum BuildType
        {
            AssetBundle,
        }

        public static string _OutputPath { get; private set; } = "D:/";
        public static string _BundlePath { get; private set; } = Application.dataPath + "/Bundle";
        public static string _ABExtension { get; } = ".ab";

        private static BuildType _curType = BuildType.AssetBundle;
        public static BuildType _CurType
        {
            get => _curType;
            set
            {
                _curType = value;
            }
        }
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

        public static string GetAllNeedBuildFileExtension()
        {
            return ".png";
        }
    }
}
