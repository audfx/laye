using System;
using System.Collections.Generic;

using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private AbstractNode? AParseDeclaration()
        {
            if (IsEoF) return null;

            int failIndex = m_tokenIndex;

            if (AParseType(null, out var typeActionsTaken) is ANodeType typeNode)
            {
                if (Current is IdentifierToken || Current is OperatorToken op)
                {
                }

                return null;
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
