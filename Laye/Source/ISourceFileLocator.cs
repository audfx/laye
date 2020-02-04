namespace Laye.Source
{
    public interface ISourceFileLocator
    {
        SourceFileDescription GetSourceFile(string relativePath);
    }
}
