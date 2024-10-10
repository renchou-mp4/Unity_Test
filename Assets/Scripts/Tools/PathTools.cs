namespace Tools
{
    public static class PathTools
    {
        public static string ReplacePathBackslash(this string path)
        {
            return path.Replace("\\", "/");
        }

        public static string RelativeToAssetPath(this string path)
        {
            return path.Substring(path.LastIndexOf("Asset"));
        }
    }
}
