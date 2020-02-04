using System.IO;

namespace Laye.Source.FileSystem
{
    public sealed class FileSystemSourceFileLocator : ISourceFileLocator
    {
        private static Stream OpenFileSystemSourceFile(SourceFileDescription sourceFileDescription)
        {
            return File.OpenRead(sourceFileDescription.FilePath);
        }

        /// <summary>
        /// The root location to find files in.
        /// If null, file paths given are relative to the current working directory instead.
        /// </summary>
        public readonly string? RootPath;

        public bool IsRelativeToCurrentWorkingDirectory => RootPath == null;

        public FileSystemSourceFileLocator(string? rootPath = null)
        {
            RootPath = rootPath;
        }

        /// <summary>
        /// Gets a usable file path in the context of the current RootPath.
        /// If a RootPath is set, returns the combination of that with the given relative path.
        /// If there is no RootPath, the relative path is returned as-is and will therefore be
        ///  treated as relative to the current working directory.
        /// </summary>
        public string GetSourceFilePathInContext(string relativePath)
        {
            if (RootPath == null)
                return relativePath;
            else return Path.Combine(RootPath, relativePath);
        }

        public SourceFileDescription GetSourceFile(string relativePath)
        {
            string filePath = GetSourceFilePathInContext(relativePath);
            return new SourceFileDescription(filePath, OpenFileSystemSourceFile);
        }
    }
}
