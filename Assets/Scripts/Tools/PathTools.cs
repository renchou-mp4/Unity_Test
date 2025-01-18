using System.IO;
using System.Linq;

namespace Tools
{
    /// <summary>
    ///     只包含通用的路径处理方法，特定功能的路径处理放到对应的tools下
    /// </summary>
    public static class PathTools
    {
        /// <summary>
        ///     将\\替换为/
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePathBackslash(this string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        ///     将\\替换为/，不包含拓展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePathBackslashWithoutExtension(this string path)
        {
            return path.Remove(path.LastIndexOf('.')).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        ///     assetPath是否以指定的路径开始
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="speccificPaths"></param>
        /// <returns></returns>
        public static bool IsStartWith(string assetPath, string[] speccificPaths)
        {
            return speccificPaths.Any(assetPath.StartsWith);
        }
    }
}