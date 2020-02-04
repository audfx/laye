using System;
using System.Collections.Generic;
using System.Diagnostics;
using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        class TokenListOperatorSplitAction
        {
            private readonly List<Token> m_tokens;
            private readonly int m_index;

            private readonly string m_imageSplit;
            private readonly OperatorToken m_originalToken;

            public TokenListOperatorSplitAction(List<Token> tokens, int where, string imageSplit)
            {
                m_tokens = tokens;
                m_index = where;

                m_imageSplit = imageSplit;
                m_originalToken = (OperatorToken)tokens[where];
            }

            public void Do()
            {
                (int Line, int Column) loc = m_originalToken.Location;

                var firstPart = new OperatorToken(loc, m_imageSplit);
                var secondPart = new OperatorToken((loc.Line, loc.Column + m_imageSplit.Length), m_originalToken.Image.Substring(m_imageSplit.Length));

                m_tokens.Insert(m_index, firstPart);
                m_tokens[m_index + 1] = secondPart;
            }

            public void Undo()
            {
                m_tokens.RemoveAt(m_index + 1);
                m_tokens[m_index] = m_originalToken;
            }
        }

        private List<Token>? m_tokens;
        private int m_tokenIndex;

        private bool IsEoF => m_tokens == null || m_tokenIndex >= m_tokens.Count;

        // NOTE(local): these require m_tokens to exist and don't check for it, will crash if not properly in initialized.
        private Token Current => m_tokens![m_tokenIndex];
        private Token? NextToken => m_tokenIndex + 1 >= m_tokens!.Count ? null : m_tokens[m_tokenIndex + 1];

        private (int, int) CurrentLocation => Current.Location;

        public Parser()
        {
        }

        private void Advance()
        {
            if (IsEoF) return;
            m_tokenIndex++;
        }

        private bool CheckOperatorStartsWith(string opImage) => Current is OperatorToken op && op.Image.StartsWith(opImage);
        private TokenListOperatorSplitAction SplitOperatorSubImage(string opImage)
        {
            Debug.Assert(CheckOperatorStartsWith(opImage), "There wasn't a valid operator to subdivide");

            var action = new TokenListOperatorSplitAction(m_tokens!, m_tokenIndex, opImage);
            action.Do();

            return action;
        }

        public List<AbstractNode> GetAbstractTree(List<Token> tokens)
        {
            var result = new List<AbstractNode>();
            
            m_tokens = tokens;
            m_tokenIndex = 0;

            while (!IsEoF && AParseTopLevelStatement() is AbstractNode aNode)
            {
                result.Add(aNode);
            }
            
            m_tokens = null;
            return result;
        }
    }
}
