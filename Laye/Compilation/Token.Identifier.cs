namespace Laye.Compilation
{
    public sealed class IdentifierToken : Token
    {
        public readonly string Image;

        public IdentifierToken((int, int) location, string image)
            : base(location)
        {
            Image = image;
        }
    }

    public sealed class OperatorToken : Token
    {
        public readonly string Image;

        public OperatorToken((int, int) location, string image)
            : base(location)
        {
            Image = image;
        }
    }
}
