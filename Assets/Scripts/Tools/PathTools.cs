using System;
using System.IO;

namespace Tools
{
    public static class PathTools
    {
        /// <summary>
        /// 将\\替换为/
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePathBackslash(this string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        /// 将\\替换为/，不包含拓展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePathBackslashWithoutExtension(this string path)
        {
            return path.Remove(path.LastIndexOf('.')).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        /// 将路径变为Asset目录开始的相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RelativeToAssetPath(this string path)
        {
            //..语法糖
            return path[path.IndexOf("Asset", StringComparison.Ordinal)..];
        }

        /// <summary>
        /// 切割路径到Bundle，取Bundle后的结果
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RelativeToBundlePathWithoutBundle(this string path)
        {
            return path[(path.IndexOf("Bundle", StringComparison.Ordinal) + 7)..];//+7是因为有一个/
        }
    }
}
