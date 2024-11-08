namespace Tools
{
    public static class PathTools
    {
        public static string ReplacePathBackslash(this string path)
        {
            return path.Replace("\\", "/");
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
