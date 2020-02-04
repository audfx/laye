using System.IO;

namespace Laye.Source
{
    public delegate Stream OpenSourceFileStream(SourceFileDescription sourceFileDescription);

    public readonly struct SourceFileDescription
    {
        public readonly string FilePath;

        private readonly OpenSourceFileStream m_openSourceFileStreamCallback;

        public SourceFileDescription(string filePath, OpenSourceFileStream callback)
        {
            FilePath = filePath;
            m_openSourceFileStreamCallback = callback;
        }

        public Stream OpenFile() => m_openSourceFileStreamCallback(this);
    }
}
