using System.Collections.Generic;

using Laye.Compilation;

namespace Laye.Syntax.Abstract
{
    public sealed class ANodeBindingDecl : AbstractNode
    {
        public ANodeType DeclType;
        public IdentifierToken Name;
        public ANodeExpression? Value;

        public ANodeBindingDecl((int, int) location, ANodeType declType, IdentifierToken name, ANodeExpression? value = null)
            : base(location)
        {
            DeclType = declType;
            Name = name;
            Value = value;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }

    public sealed class ANodeFunctionDecl : AbstractNode
    {
        public ANodeType ReturnType;
        public Token FunctionName;
        public List<ANodeBindingDecl> ParameterBindings;

        public ANodeFunctionDecl((int, int) location, ANodeType returnType, Token functionName, List<ANodeBindingDecl> paramBindings)
            : base(location)
        {
            ReturnType = returnType;
            FunctionName = functionName;
            ParameterBindings = paramBindings;
        }

        public override void Accept(AbstractSyntaxVisitor visitor) => visitor.Visit(this);
        public override T Accept<T>(AbstractSyntaxVisitor<T> visitor) => visitor.Visit(this);
    }
}
