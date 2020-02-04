using System;
using System.Collections.Generic;
using Laye.Compilation;

namespace Laye.Syntax.Abstract
{
    public abstract class ANodeExpression : AbstractNode
    {
        protected ANodeExpression((int, int) location)
            : base(location)
        {
        }
    }

    public class ANodeIntegerLiteral : ANodeExpression
    {
        public IntegerToken Integer;

        public ANodeIntegerLiteral(IntegerToken itok)
            : base(itok.Location)
        {
            Integer = itok;
        }
    }

    public class ANodeFloatLiteral : ANodeExpression
    {
        public FloatToken Float;

        public ANodeFloatLiteral(FloatToken ftok)
            : base(ftok.Location)
        {
            Float = ftok;
        }
    }

    public class ANodeStringLiteral : ANodeExpression
    {
        public StringToken String;

        public ANodeStringLiteral(StringToken stok)
            : base(stok.Location)
        {
            String = stok;
        }
    }

    public class ANodeBoolLiteral : ANodeExpression
    {
        public KeywordToken Bool;

        public ANodeBoolLiteral(KeywordToken kwtok)
            : base(kwtok.Location)
        {
            Bool = kwtok;
        }
    }

    public class ANodeNullLiteral : ANodeExpression
    {
        public KeywordToken Null;

        public ANodeNullLiteral(KeywordToken kwtok)
            : base(kwtok.Location)
        {
            Null = kwtok;
        }
    }

    public class ANodeIdentifierExpression : ANodeExpression
    {
        public IdentifierToken Identifier;

        public ANodeIdentifierExpression(IdentifierToken ident)
            : base(ident.Location)
        {
            Identifier = ident;
        }
    }
}
