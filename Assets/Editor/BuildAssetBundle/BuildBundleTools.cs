using UnityEngine;

namespace Tools
{
    public static class BuildBundleTools
    {
        public enum BuildType
        {
            AssetBundle,
        }

        public static string _OutputPath { get; private set; } = Application.streamingAssetsPath + "/AssetBundle";
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
        /// ��ȡ��Ҫ�������Դ��׺
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
        /// ��ȡ��Ŀ¼����AB������Դ·��
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
        /// ��·���Ƿ���ָ����Asset·����ͷ
        /// </summary>
        /// <param name="filePath">�Ƿ�ΪAsset��ʼ������</param>
        /// <param name="specifiedPath"></param>
        /// <returns></returns>
        public static bool IsStartWithSpecifiedAssetPath(string filePath, params string[] specifiedPath)
        {
            string assetFilePath = GetAssetPath(filePath);
            foreach (string path in specifiedPath)
            {
                if (assetFilePath.StartsWith(path))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ��·����ΪAssetĿ¼��ʼ�����·��
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetAssetPath(string filePath)
        {
            return filePath.Substring(filePath.IndexOf("Asset"));
        }
    }
}
