public static class FileTools
{
    public static string GetFileNameWithExtension(string filePath)
    {
        return filePath.Substring(filePath.LastIndexOf('/'));
    }

    public static string GetFileNameWithoutExtension(string filePath)
    {
        int startIndex = filePath.LastIndexOf('/');
        return filePath.Substring(startIndex, filePath.LastIndexOf('.') - startIndex);
    }
}
