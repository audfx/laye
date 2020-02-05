using System;
using System.Collections.Generic;
using System.Linq;
using Laye.Syntax.Abstract;

namespace Laye.Compilation
{
    public sealed partial class Parser
    {
        private BuiltInType GetBuiltInTypeFromKind(KeywordKind kind) => kind switch
        {
            KeywordKind.Void => BuiltInType.Void,
            KeywordKind.Bool => BuiltInType.Bool,
            KeywordKind.String => BuiltInType.String,

            KeywordKind.UByte => BuiltInType.UByte,
            KeywordKind.UShort => BuiltInType.UShort,
            KeywordKind.UInt => BuiltInType.UInt,
            KeywordKind.ULong => BuiltInType.ULong,

            KeywordKind.Byte => BuiltInType.Byte,
            KeywordKind.Short => BuiltInType.Short,
            KeywordKind.Int => BuiltInType.Int,
            KeywordKind.Long => BuiltInType.Long,

            KeywordKind.Float => BuiltInType.Float,
            KeywordKind.Double => BuiltInType.Double,
            KeywordKind.Decimal => BuiltInType.Decimal,

            _ => BuiltInType.Invalid,
        };

        private bool IsKeywordBuiltInType(KeywordKind kind) => GetBuiltInTypeFromKind(kind) != BuiltInType.Invalid;

        private ANodeType? AParseType(Stack<OpSplitAction>? actionStack, out Stack<OpSplitAction> actionsTaken)
        {
            var stack = actionStack ?? new Stack<OpSplitAction>();

            int failMark = m_tokenIndex;
            (int, int) location = CurrentLocation;

            ANodeType? typeNode = null;
            if (Current is KeywordToken kw && IsKeywordBuiltInType(kw.Kind))
            {
                Advance(); // the keyword
                typeNode = new ANodeBuiltInType(location, GetBuiltInTypeFromKind(kw.Kind));
            }
            else if (CheckOperatorStartsWith("*"))
            {
                var undoAction = SplitOperatorSubImage("*");
                Advance(); // the start of that split image

                var containedType = AParseType(stack, out actionsTaken);
                if (containedType == null)
                    undoAction?.Undo(); // will undo the split, but won't itself reset the token position. We do that later ourselves, it doesn't manage it for us.
                else
                {
                    if (undoAction != null)
                        actionsTaken.Push(undoAction); // now that we've commited to the action, add it to our action stack.
                    typeNode = new ANodePointerType(location, containedType);
                }
            }
            else if (Current is DelimiterToken delim)
            {
                if (delim.Kind == DelimiterKind.OpenParen)
                {
                    Advance();

                    var elementStack = new Stack<OpSplitAction>();
                    var elementTypes = new List<ANodeType>();
                    var elementNames = new List<IdentifierToken?>();

                    bool parsed = true;
                    while (parsed)
                    {
                        if (IsEoF)
                        {
                            parsed = false;
                            break;
                        }

                        var elementType = AParseType(elementStack, out var _); // out is the same as the given elementStack
                        if (elementType is ANodeType elementNode)
                        {
                            elementTypes.Add(elementType);
                            if (Current is IdentifierToken elementName)
                            {
                                Advance(); // ident
                                elementNames.Add(elementName);
                            }
                            else elementNames.Add(null);
                        }
                        else
                        {
                            parsed = false;
                            break;
                        }

                        if (Current is DelimiterToken checkDelim && checkDelim.Kind == DelimiterKind.Comma)
                            Advance();
                        else break;
                    }

                    if (parsed && Current is DelimiterToken delimClose && delimClose.Kind == DelimiterKind.CloseParen)
                    {
                        Advance();
                        typeNode = new ANodeTupleType(location, elementTypes.ToArray(), elementNames.ToArray());
                    }
                }
                else if (delim.Kind == DelimiterKind.OpenBracket)
                {
                    Advance();

                    if (Current is DelimiterToken checkIsListToken && checkIsListToken.Kind == DelimiterKind.CloseBracket)
                    {
                        Advance(); // the close `]`

                        var elementType = AParseType(stack, out var _);
                        if (elementType is ANodeType elementTypeNode)
                            typeNode = new ANodeListType(location, elementTypeNode);
                    }
                    else
                    {
                        var elementStack = new Stack<OpSplitAction>();
                        var dimensionLengths = new List<AbstractNode?>();

                        bool parsed = true;
                        while (parsed)
                        {
                            if (IsEoF)
                            {
                                parsed = false;
                                break;
                            }

                            if (Current is OperatorToken anyLengthOp && anyLengthOp.Image == "*")
                            {
                                Advance();
                                dimensionLengths.Add(null);
                            }
                            else
                            {
                                var dimensionLength = AParseExpression(elementStack, out var _); // out is the same as the given elementStack
                                if (dimensionLength is ANodeExpression dimLengthNode)
                                    dimensionLengths.Add(dimLengthNode);
                                else
                                {
                                    parsed = false;
                                    break;
                                }
                            }

                            if (Current is DelimiterToken checkDelim && checkDelim.Kind == DelimiterKind.Comma)
                                Advance();
                            else break;
                        }

                        if (parsed && Current is DelimiterToken delimClose && delimClose.Kind == DelimiterKind.CloseBracket)
                        {
                            Advance();

                            var elementType = AParseType(stack, out var _);
                            if (elementType is ANodeType elementTypeNode)
                                typeNode = new ANodeArrayType(location, dimensionLengths.Select(d => d!).ToArray(), elementTypeNode);
                        }
                    }
                }
            }

            if (typeNode == null)
                m_tokenIndex = failMark;

            actionsTaken = stack;
            return typeNode;
        }
    }
}
