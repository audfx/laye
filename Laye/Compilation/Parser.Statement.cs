using System;
using System.Collections.Generic;

using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private AbstractNode? ParseStatement(Stack<OpSplitAction>? actionStack, out Stack<OpSplitAction> actionsTaken)
        {
            var stack = actionStack ?? new Stack<OpSplitAction>();
            actionsTaken = stack;

            AbstractNode? result = AParseDeclaration(stack, out var _);
            if (result == null)
            {
                while (stack.Count > 0) stack.Pop().Undo();

                result = AParseExpression(stack, out var _);
                if (result == null)
                {
                    while (stack.Count > 0) stack.Pop().Undo();
                }
            }

            if (!RequireSemi())
                return null;

            return null;
        }
    }
}
