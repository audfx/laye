using System.IO;
using System.Reflection;

namespace Laye.Source.Embedded
{
    public sealed class EmbeddedResourceFileLocator : ISourceFileLocator
    {
        public readonly Assembly Assembly;
        public readonly string RootNamespace;

        public EmbeddedResourceFileLocator(Assembly assembly, string rootNamespace = "")
        {
            Assembly = assembly;
            RootNamespace = rootNamespace;
        }

        private Stream OpenResourceStream(SourceFileDescription sourceFileDescription)
        {
            string resourceName = GetSourceFileLocationInContext(sourceFileDescription.FilePath);
            return Assembly.GetManifestResourceStream(resourceName);
        }

        public string GetSourceFileLocationInContext(string relativePath)
        {
            string relativeResourceName = string.Join(".", relativePath.Split('\\', '/'));
            if (!string.IsNullOrWhiteSpace(RootNamespace))
                return $"{RootNamespace}.{relativeResourceName}";
            else return relativeResourceName;
        }

        public SourceFileDescription GetSourceFile(string relativePath)
        {
            return new SourceFileDescription(relativePath, OpenResourceStream);
        }
    }
}
