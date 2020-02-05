using System;
using System.Collections.Generic;
using System.Text;

using Laye.Syntax.Abstract;

namespace Laye.Syntax
{
    public abstract class AbstractSyntaxVisitor
    {
        public abstract void VisitDefault(AbstractNode node);

        public virtual void Visit(ANodeType node) => VisitDefault(node);
        public virtual void Visit(ANodeBuiltInType node) => VisitDefault(node);
        public virtual void Visit(ANodePointerType node) => VisitDefault(node);
        public virtual void Visit(ANodeListType node) => VisitDefault(node);
        public virtual void Visit(ANodeArrayType node) => VisitDefault(node);
        public virtual void Visit(ANodeTupleType node) => VisitDefault(node);

        public virtual void Visit(ANodeBindingDecl node) => VisitDefault(node);
        public virtual void Visit(ANodeFunctionDecl node) => VisitDefault(node);

        public virtual void Visit(ANodeIdentifierExpression node) => VisitDefault(node);
        public virtual void Visit(ANodeBoolLiteral node) => VisitDefault(node);
        public virtual void Visit(ANodeFloatLiteral node) => VisitDefault(node);
        public virtual void Visit(ANodeIntegerLiteral node) => VisitDefault(node);
        public virtual void Visit(ANodeStringLiteral node) => VisitDefault(node);
        public virtual void Visit(ANodeNullLiteral node) => VisitDefault(node);
    }

    public abstract class AbstractSyntaxVisitor<T>
    {
        public abstract T VisitDefault(AbstractNode node);

        public virtual T Visit(ANodeType node) => VisitDefault(node);
        public virtual T Visit(ANodeBuiltInType node) => VisitDefault(node);
        public virtual T Visit(ANodePointerType node) => VisitDefault(node);
        public virtual T Visit(ANodeListType node) => VisitDefault(node);
        public virtual T Visit(ANodeArrayType node) => VisitDefault(node);
        public virtual T Visit(ANodeTupleType node) => VisitDefault(node);

        public virtual T Visit(ANodeBindingDecl node) => VisitDefault(node);
        public virtual T Visit(ANodeFunctionDecl node) => VisitDefault(node);

        public virtual T Visit(ANodeIdentifierExpression node) => VisitDefault(node);
        public virtual T Visit(ANodeBoolLiteral node) => VisitDefault(node);
        public virtual T Visit(ANodeFloatLiteral node) => VisitDefault(node);
        public virtual T Visit(ANodeIntegerLiteral node) => VisitDefault(node);
        public virtual T Visit(ANodeStringLiteral node) => VisitDefault(node);
        public virtual T Visit(ANodeNullLiteral node) => VisitDefault(node);
    }
}
