using System;
using System.Collections.Generic;

using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private bool RequireSemi()
        {
            if (PreviousToken is DelimiterToken delim && delim.Kind == DelimiterKind.CloseBrace)
                return true;

            if (Current is DelimiterToken semi && semi.Kind == DelimiterKind.SemiColon)
            {
                Advance();
                return true;
            }

            return false;
        }

        private AbstractNode? AParseTopLevelStatement()
        {
            if (IsEoF) return null;

            if (Current is KeywordToken kw && kw.Kind == KeywordKind.Namespace)
            {
                // do namespace things;
                Advance(); // `namespace`
            }
            else
            {
                AbstractNode? result = AParseDeclaration(null, out var actionsTaken);
                if (result == null)
                {
                    while (actionsTaken.Count > 0) actionsTaken.Pop().Undo();
                    return null;
                }

                if (!RequireSemi())
                    return null;

                return result;
            }

            return null;
        }
    }
}
