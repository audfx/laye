using System;
using System.Collections.Generic;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private ANodeExpression? AParseExpression(Stack<TokenListOperatorSplitAction>? actionStack, out Stack<TokenListOperatorSplitAction> actionsTaken)
        {
            var stack = actionStack ?? new Stack<TokenListOperatorSplitAction>();

            ANodeExpression? result = null;

            actionsTaken = stack;
            return result;
        }
    }
}
