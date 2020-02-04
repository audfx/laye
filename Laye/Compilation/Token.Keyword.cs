namespace Laye.Compilation
{
    public sealed class KeywordToken : Token
    {
        public readonly KeywordKind Kind;

        public KeywordToken((int, int) location, KeywordKind kind)
            : base(location)
        {
            Kind = kind;
        }
    }

    public enum KeywordKind
    {
        True, False, Nil, Default,

        UByte, UShort, UInt, ULong,
        Byte, Short, Int, Long,
        Float, Double, Decimal,
        Bool, String, Void,

        If, Each, While, Do,
        Return, Break, Continue,
        Struct, Enum,

        Var, Const, Namespace, Use, Public, Private,
    }
}
