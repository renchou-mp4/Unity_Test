using UnityEngine;

namespace Tools
{
    public static class BuildBundleTools
    {
        public enum BuildType
        {
            AssetBundle,
        }

        /// <summary>
        /// AB包输出路径
        /// </summary>
        public static string _OutputPath { get; private set; } = Application.streamingAssetsPath + "/Bundle";
        /// <summary>
        /// Manifest文件输出路径
        /// </summary>
        public static string _ManifestOutputPath { get; set; } = Application.dataPath + "/asset_version.json";
        /// <summary>
        /// Bundle文件夹路径
        /// </summary>
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

        /// <summary>
        /// 获取不需要打包的资源后缀
        /// </summary>
        /// <returns></returns>
        public static string[] GetNoNeedBuildFileExtension()
        {
            return new string[]
            {
                ".meta",
            };
        }

        /// <summary>
        /// 获取按目录构建AB包的资源路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetNeedBuildPathByDirectory()
        {
            return new string[]
            {
                "Assets/Bundle/Sprites",
            };
        }

        /// <summary>
        /// 获取按单个文件构建AB包的资源路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetNeedBuildPathBySingle()
        {
            return new string[]
            {
                "Assets/Bundle/Fonts",
            };
        }

        /// <summary>
        /// 该路径是否以指定的Asset路径开头
        /// </summary>
        /// <param name="filePath">是否为Asset开始都可以</param>
        /// <param name="specifiedPath"></param>
        /// <returns></returns>
        public static bool IsStartWithSpecifiedAssetPath(string filePath, params string[] specifiedPath)
        {
            string assetFilePath = filePath.RelativeToAssetPath();
            foreach (string path in specifiedPath)
            {
                if (assetFilePath.StartsWith(path))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取Bundle的名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetBundleName(string path)
        {
            //7是加了/
            return path.Substring(path.IndexOf("Bundle") + 7);
        }
    }
}
