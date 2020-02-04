using System;
using System.Collections.Generic;

using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
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
                var result = AParseDeclaration();
                return result;
            }

            return null;
        }
    }
}
