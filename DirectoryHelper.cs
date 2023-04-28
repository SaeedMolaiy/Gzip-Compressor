namespace GzipCompress
{
    /// <summary>
    /// This class contains a single method that determines if a path is a directory path or a file path.
    /// </summary>
    public static class DirectoryHelper
    {
        public static bool IsDirectory(string path)
        {
            var attributes = File.GetAttributes(path);
            return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}