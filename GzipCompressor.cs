using System.IO.Compression;

namespace GzipCompress
{
    public class GzipCompressor
    {
        /// <summary>
        /// Compresses a file or directory using Gzip compression.
        /// </summary>
        /// <param name="sourcePath">The path to the file or directory to compress.</param>
        /// <param name="destinationPath">The path to save the compressed file.</param>
        /// <returns>True if compression is successful, false otherwise.</returns>
        public bool Compress(string sourcePath, string destinationPath)
        {
            try
            {
                if (DirectoryHelper.IsDirectory(sourcePath))
                {
                    CreateGzipFile(sourcePath, destinationPath);
                }
                else
                {
                    CreateGzipFileFromDirectory(sourcePath, destinationPath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Compresses a single file using Gzip compression.
        /// </summary>
        /// <param name="sourcePath">The path to the file to compress.</param>
        /// <param name="destinationPath">The path to save the compressed file.</param>
        private static void CreateGzipFile(string sourcePath, string destinationPath)
        {
            // Open the source file and create the destination file
            using var sourceStream = new FileStream(sourcePath, FileMode.Open);
            using var destinationStream = new FileStream(destinationPath, FileMode.Create);

            // Create the Gzip stream and copy the source stream to it
            using var gzipStream = new GZipStream(destinationStream, CompressionMode.Compress);

            sourceStream.CopyTo(gzipStream);
        }

        /// <summary>
        /// Compresses a directory and all of its contents using Gzip compression.
        /// </summary>
        /// <param name="sourcePath">The path to the directory to compress.</param>
        /// <param name="destinationPath">The path to save the compressed file.</param>
        private static void CreateGzipFileFromDirectory(string sourcePath, string destinationPath)
        {
            // Open the destination file and create the Gzip archive
            using var zipToOpen = new FileStream(destinationPath, FileMode.Create);
            using var archive = new GZipStream(zipToOpen, CompressionLevel.Optimal);

            // Iterate over all files in the source directory and its subdirectories
            foreach (var file in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                // Open the file and copy it to the Gzip archive
                using var fileStream = new FileStream(file, FileMode.Open);

                var buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    archive.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}