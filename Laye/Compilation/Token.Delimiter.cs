namespace Laye.Compilation
{
    public sealed class DelimiterToken : Token
    {
        public readonly DelimiterKind Kind;

        public DelimiterToken((int, int) location, DelimiterKind kind)
            : base(location)
        {
            Kind = kind;
        }
    }

    public enum DelimiterKind
    {
        Dot = '.',
        Comma = ',',
        SemiColon = ';',
        Colon = ':',
        OpenParen = '(',
        CloseParen = ')',
        OpenBracket = '[',
        CloseBracket = ']',
        OpenBrace = '{',
        CloseBrace = '}',
    }
}
