using System;
using System.Numerics;

namespace Laye.Compilation
{
    public abstract class TokenLiteral : Token
    {
        protected TokenLiteral((int, int) location)
            : base(location)
        {
        }
    }

    public sealed class BoolToken : TokenLiteral
    {
        public readonly bool Value;

        public BoolToken((int, int) location, bool value)
            : base(location)
        {
            Value = value;
        }
    }

    public sealed class IntegerToken : TokenLiteral
    {
        public readonly BigInteger Value;
        public readonly string Image;

        public IntegerToken((int, int) location, BigInteger value, string image)
            : base(location)
        {
            Value = value;
            Image = image;
        }
    }

    public sealed class FloatToken : TokenLiteral
    {
        public readonly BigDecimal Value;
        public readonly string Image;

        public FloatToken((int, int) location, BigDecimal value, string image)
            : base(location)
        {
            Value = value;
            Image = image;
        }
    }

    public sealed class StringToken : TokenLiteral
    {
        private readonly byte[] m_byteValue;
        public ReadOnlySpan<byte> ByteValue => m_byteValue;

        public readonly string StringValue;

        public StringToken((int, int) location, byte[] byteValue, string stringValue)
            : base(location)
        {
            m_byteValue = byteValue;
            StringValue = stringValue;
        }
    }
}
