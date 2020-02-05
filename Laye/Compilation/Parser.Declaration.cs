using System;
using System.Collections.Generic;

using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private AbstractNode? AParseDeclaration(Stack<OpSplitAction>? actionStack, out Stack<OpSplitAction> actionsTaken)
        {
            var stack = actionStack ?? new Stack<OpSplitAction>();
            actionsTaken = stack;

            if (IsEoF) return null;

            (int, int) location = Current.Location;
            int failIndex = m_tokenIndex;

            if (AParseType(null, out var typeActionsTaken) is ANodeType typeNode)
            {
                if (Current is IdentifierToken || Current is OperatorToken)
                {
                    if (Current is IdentifierToken ident && !(NextToken is DelimiterToken maybeOpenParen && maybeOpenParen.Kind == DelimiterKind.OpenParen))
                    {
                        Advance(); // the ident

                        ANodeExpression? value = null;
                        if (Current is OperatorToken maybeEqOpToken && maybeEqOpToken.Image == "=")
                        {
                            Advance(); // exactly an `=`

                            value = AParseExpression(actionStack, out var _);
                            if (value is null)
                                return null;
                        }

                        // also check for function stuffs
                        return new ANodeBindingDecl(location, typeNode, ident, value);
                    }
                    else
                    {
                        // otherwise JUST check for function decls
                    }
                }
            }
            else
            {
                while (typeActionsTaken.Count > 0)
                    typeActionsTaken.Pop().Undo();
                m_tokenIndex = failIndex;
            }

            return null;
        }
    }
}
