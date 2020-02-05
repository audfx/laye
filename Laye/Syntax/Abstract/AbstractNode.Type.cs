using System;
using System.Collections.Generic;
using System.Text;
using Laye.Compilation;

namespace Laye.Syntax.Abstract
{
    public abstract class ANodeType : AbstractNode
    {
        protected ANodeType((int, int) location)
            : base(location)
        {
        }
    }

    public class ANodeBuiltInType : ANodeType
    {
        public BuiltInType BuiltInType;

        public ANodeBuiltInType((int, int) location, BuiltInType type)
            : base(location)
        {
            BuiltInType = type;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodePointerType : ANodeType
    {
        public ANodeType Pointee;

        public ANodePointerType((int, int) location, ANodeType pointee)
            : base(location)
        {
            Pointee = pointee;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeArrayType : ANodeType
    {
        public AbstractNode[] ArrayLengthNodes;
        public ANodeType ElementType;

        public ANodeArrayType((int, int) location, AbstractNode[] lengths, ANodeType elementType)
            : base(location)
        {
            ArrayLengthNodes = lengths;
            ElementType = elementType;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeListType : ANodeType
    {
        public ANodeType ElementType;

        public ANodeListType((int, int) location, ANodeType elementType)
            : base(location)
        {
            ElementType = elementType;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public class ANodeTupleType : ANodeType
    {
        public ANodeType[] ElementTypes;
        public IdentifierToken?[] OptElementNames;

        public ANodeTupleType((int, int) location, ANodeType[] elementTypes, IdentifierToken?[] optElementNames)
            : base(location)
        {
            ElementTypes = elementTypes;
            OptElementNames = optElementNames;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }
}
