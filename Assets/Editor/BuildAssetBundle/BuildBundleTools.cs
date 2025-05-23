﻿using System;
using System.Linq;
using UnityEngine;

namespace Editor.BuildAssetBundle
{
    public static class BuildBundleTools
    {
        public enum BuildType
        {
            AssetBundle
        }

        private static IBuildBundle _curBuilding;

        /// <summary>
        ///     AB包输出路径
        /// </summary>
        public static string _OutputPath { get; private set; } = Application.streamingAssetsPath + "/Bundle";

        /// <summary>
        ///     Manifest文件输出路径
        /// </summary>
        public static string _ManifestOutputPath { get; set; } = Application.dataPath + "/asset_version.json";

        /// <summary>
        ///     Bundle文件夹路径
        /// </summary>
        public static string _BundlePath { get; private set; } = Application.dataPath + "/Bundle";

        /// <summary>
        ///     当前构建类型
        /// </summary>
        public static BuildType _CurType { get; set; } = BuildType.AssetBundle;


        public static void BuildBundle()
        {
            switch (_CurType)
            {
                case BuildType.AssetBundle:
                    _curBuilding = new BuildAssetBundle();
                    _curBuilding.Build();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     获取不需要打包的资源后缀
        /// </summary>
        /// <returns></returns>
        public static string[] GetNoNeedBuildFileExtension()
        {
            return new[]
            {
                ".meta"
            };
        }

        /// <summary>
        ///     获取按目录构建AB包的资源路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetNeedBuildPathByDirectory()
        {
            //路径由深到浅添加
            return new[]
            {
                "Assets/Bundle/Sprites"
            };
        }

        /// <summary>
        ///     该路径是否以指定的Asset路径开头
        /// </summary>
        /// <param name="filePath">是否为Asset开始都可以</param>
        /// <param name="specifiedPath">指定路径</param>
        /// <returns></returns>
        public static bool IsStartWithSpecifiedAssetPath(string filePath, params string[] specifiedPath)
        {
            string assetFilePath = filePath.RelativeToAssetPath();
            return specifiedPath.Any(path => assetFilePath.StartsWith(path));
        }

        /// <summary>
        ///     将路径变为Asset目录开始的相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RelativeToAssetPath(this string path)
        {
            //..语法糖
            return path[path.IndexOf("Asset", StringComparison.Ordinal)..];
        }

        /// <summary>
        ///     切割路径到Bundle，取Bundle后的结果
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RelativeToBundlePathWithoutBundle(this string path)
        {
            return path[(path.IndexOf("Bundle", StringComparison.Ordinal) + 7)..]; //+7是因为有一个/
        }
    }
}