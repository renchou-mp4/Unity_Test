﻿using System.IO;

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
        /// 将路径变为Asset目录开始的相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RelativeToAssetPath(this string path)
        {
            return path.Substring(path.IndexOf("Asset"));
        }
    }
}
