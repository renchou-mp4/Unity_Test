using System.IO;
using System.Linq;

namespace Tools
{
    public static class FileTools
    {
        /// <summary>
        ///     获取带扩展名的文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithExtension(this string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <summary>
        ///     获取不带扩展名的文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        ///     获取文件夹名称
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string GetDirectoryName(this string directoryPath)
        {
            return Path.GetDirectoryName(directoryPath);
        }

        /// <summary>
        ///     获取扩展名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetExtension(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        ///     文件是否是指定的扩展名
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool IsEndWith(this string filePath, params string[] extension)
        {
            return extension.ToList().Contains(filePath.GetExtension());
        }
    }
}