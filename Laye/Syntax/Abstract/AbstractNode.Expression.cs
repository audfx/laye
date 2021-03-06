﻿using System;
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

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeFloatLiteral : ANodeExpression
    {
        public FloatToken Float;

        public ANodeFloatLiteral(FloatToken ftok)
            : base(ftok.Location)
        {
            Float = ftok;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeStringLiteral : ANodeExpression
    {
        public StringToken String;

        public ANodeStringLiteral(StringToken stok)
            : base(stok.Location)
        {
            String = stok;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeBoolLiteral : ANodeExpression
    {
        public KeywordToken Bool;

        public ANodeBoolLiteral(KeywordToken kwtok)
            : base(kwtok.Location)
        {
            Bool = kwtok;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeNullLiteral : ANodeExpression
    {
        public KeywordToken Null;

        public ANodeNullLiteral(KeywordToken kwtok)
            : base(kwtok.Location)
        {
            Null = kwtok;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeIdentifierExpression : ANodeExpression
    {
        public IdentifierToken Identifier;

        public ANodeIdentifierExpression(IdentifierToken ident)
            : base(ident.Location)
        {
            Identifier = ident;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }
}
