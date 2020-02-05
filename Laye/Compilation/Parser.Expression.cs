using System;
using System.Collections.Generic;

using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private ANodeExpression? AParseExpression(Stack<OpSplitAction>? actionStack, out Stack<OpSplitAction> actionsTaken)
        {
            var stack = actionStack ?? new Stack<OpSplitAction>();

            ANodeExpression? result = null;

            result = ParsePrimaryExpression(stack, out var _);

            actionsTaken = stack;
            return result;
        }

        private ANodeExpression? ParsePrimaryExpression(Stack<OpSplitAction>? actionStack, out Stack<OpSplitAction> actionsTaken)
        {
            var stack = actionStack ?? new Stack<OpSplitAction>();

            ANodeExpression? result = null;

            if (Current is IntegerToken itok)
            {
                Advance();
                result = new ANodeIntegerLiteral(itok);
            }

            actionsTaken = stack;
            return result;
        }
    }
}
