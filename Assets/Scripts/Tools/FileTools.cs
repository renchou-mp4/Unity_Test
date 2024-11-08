using System.Linq;

namespace Tools
{
    public static class FileTools
    {
        public static string GetFileNameWithExtension(this string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf('/') + 1);
        }

        public static string GetFileNameWithoutExtension(this string filePath)
        {
            int startIndex = filePath.LastIndexOf('/') + 1;
            return filePath.Substring(startIndex, filePath.LastIndexOf('.') - startIndex);
        }

        public static string GetDirectoryName(this string directoryPath)
        {
            return directoryPath.Substring(directoryPath.LastIndexOf("/") + 1);
        }

        public static string Extension(this string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf('.'));
        }

        public static bool IsEndWith(this string filePath, params string[] extension)
        {
            return extension.ToList().Contains(filePath.Extension());
        }

    }
}
