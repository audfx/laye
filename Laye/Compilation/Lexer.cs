using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace Laye.Compilation
{
    public class Lexer
    {
        private static readonly Dictionary<string, KeywordKind> m_kws = new Dictionary<string, KeywordKind>();

        static Lexer()
        {
            string[] names = Enum.GetNames(typeof(KeywordKind));
            foreach (string name in names)
                m_kws[name.ToLower()] = (KeywordKind)Enum.Parse(typeof(KeywordKind), name);
        }

        private static bool IsOperatorCharacter(char c) =>
            c == '+' || c == '-' || c == '*' || c == '/' || c == '%' ||
            c == '=' || c == '!' || c == '~' || c == '?' || c == '@' ||
            c == '&' || c == '|' || c == '^' || c == '#' || c == '`';

        private StreamReader? m_reader;
        private char m_current = '\0';
        private char? m_peeked = null;
        private int m_line = 1, m_column = 1;

        private bool IsEoF => m_current == '\0';

        public Lexer()
        {
        }

        public List<Token> GetTokens(string inputSource)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputSource));
            return GetTokens(stream);
        }

        /// <summary>
        /// Takes ownership of the input stream.
        /// </summary>
        /// <param name="input"></param>
        public List<Token> GetTokens(Stream input)
        {
            m_reader = new StreamReader(input, Encoding.UTF8);
            m_line = m_column = 1;

            Advance();
            EatWhiteSpace();

            var tokens = new List<Token>();
            while (GetNextToken() is Token token)
            {
                tokens.Add(token);
            }

            m_reader.Dispose();
            m_reader = null;

            return tokens;
        }

        private void Advance()
        {
            Debug.Assert(m_reader != null, "Reader cannot be null when advancing.");

            int next = m_peeked ?? m_reader!.Read();
            m_peeked = null;

            if (next < 0)
                m_current = '\0';
            else
            {
                if (m_current == '\n')
                {
                    m_line++;
                    m_column = 1;
                }

                m_current = (char)next;
            }
        }

        private char Peek()
        {
            Debug.Assert(m_reader != null, "Reader cannot be null when peeking.");

            if (m_peeked is char peeked)
                return peeked;

            int next = m_reader!.Read();
            if (next >= 0)
                m_peeked = (char)next;
            else m_peeked = null;

            return m_peeked ?? '\0';
        }

        private void EatWhiteSpace()
        {
            while (!IsEoF)
            {
                if (char.IsWhiteSpace(m_current))
                    Advance();
                else if (m_current == '\\' && Peek() == '\\')
                {
                    Advance(); // first slash
                    Advance(); // second slash

                    while (!IsEoF && m_current != '\n') Advance(); // the rest of the line

                    // the newline if it exists will be eaten by the whitespace loop next iteration.
                }
                else if (m_current == '\\' && Peek() == '*')
                {
                    Advance(); // first slash
                    Advance(); // first star

                    while (!IsEoF && m_current != '*' && Peek() != '\\') Advance(); // everything until the */

                    Advance(); // second star
                    Advance(); // second slash
                }
                else break;
            }
        }

        private Token? GetNextToken()
        {
            if (IsEoF || m_current == '\0') return null;

            Token? result = null;
            char c = m_current;

            (int, int) location = (m_line, m_column);

            switch (c)
            {
                case '.': case ',':
                case ':': case ';':
                case '[': case ']':
                case '(': case ')':
                case '{': case '}':
                {
                    Advance();
                    result = new DelimiterToken(location, (DelimiterKind)c);
                } break;

                case '$': throw new NotImplementedException(); // interpolated string literals
                case '"':
                {
                    result = ReadString();
                } break;

                default:
                {
                    if (char.IsDigit(c) || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
                        result = ReadNumberOrIdentifierOrKeyword();
                    else if (IsOperatorCharacter(c))
                    {
                        var operatorImage = new StringBuilder();
                        while (!IsEoF && IsOperatorCharacter(m_current))
                        {
                            operatorImage.Append(m_current);
                            Advance();
                        }
                        result = new OperatorToken(location, operatorImage.ToString());
                    }
                } break;
            }

            EatWhiteSpace();
            return result;
        }

        private Token ReadNumberOrIdentifierOrKeyword()
        {
            (int, int) location = (m_line, m_column);

            static bool IsValidNumberOrIdentifierChar(char c) => char.IsDigit(c) || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
            static bool IsCharInRadix(char c, int radix)
            {
                if (radix <= 10)
                    return c >= '0' && c < radix + '0';
                else
                {
                    return (c >= '0' && c <= '9') ||
                           (c >= 'a' && c < 'a' + radix - 10) ||
                           (c >= 'A' && c < 'A' + radix - 10);
                }
            }

            static BigInteger ParseBigInteger(string s, uint b)
            {
                checked
                {
                    BigInteger result = 0;
                    foreach (char c in s)
                    {
                        uint num;
                        if (c >= '0' && c <= '9')
                            num = c - (uint)'0';
                        else if (c >= 'A' && c <= 'Z')
                            num = c - (uint)'A' + 10;
                        else if (c >= 'a' && c <= 'z')
                            num = c - (uint)'a' + 10;
                        else throw new ArgumentException("The string contains an invalid character '" + c + "'", "s");

                        if (num >= b) throw new ArgumentException("The string contains a character '" + c + "' which is not allowed in base " + b, "s");
                        result = result * b + num;
                    }

                    return result;
                }
            }

            var builder = new StringBuilder();

            int? numberRadix = null;
            string? radixImage = null;

            bool isIdentifier = !char.IsDigit(m_current); // if it starts with a non-digit char then obv it's not a digit
            bool isFractional = false; // when a `.` occurs in a standard numeric literal, switch to fractional literal mode

            while (numberRadix is int r ? // can only be a numeric literal
                   (IsCharInRadix(m_current, r) || m_current == '_') : // check numeric literal stuff
                   (IsValidNumberOrIdentifierChar(m_current) || m_current == '$' || m_current == '.' || (isFractional && m_current == 'e'))) // check number/identifier chars or switch chars to number only
            {
                char c = m_current;
                if (m_current == '.') // switch to fractional BEFORE appending the character
                {
                    if (isFractional || isIdentifier || numberRadix is int)
                        break; // `.` will become an index operator rather than a fractional separator when already an identifier or in a special-radix integer (special-radix fractionals don't exist)

                    isFractional = true;
                }

                builder.Append(c);
                Advance();

                if (c == '$') // switch to radix AFTER appending, as it's an error afterward anyway
                {
                    if (isIdentifier) // can't switch from identifier mode to radix-integer mode
                        throw new NotImplementedException();

                    radixImage = builder.ToString(); // tostring it and remove the '#'
                    builder.Clear();

                    radixImage = radixImage.Substring(0, radixImage.Length - 1);
                    if (radixImage.Contains("_")) // disallow underscores in radix declarations
                        throw new NotImplementedException();

                    numberRadix = int.Parse(radixImage);
                    if (numberRadix.Value < 2 || numberRadix.Value > 36) // radix must be in the range [2, 36]
                        throw new NotImplementedException();
                }
                else if (c == 'e')
                {
                    isFractional = true;

                    if (m_current == '-')
                    {
                        builder.Append('-');
                        Advance();
                    }
                }
                else if (!isFractional && !numberRadix.HasValue && !char.IsDigit(c)) // if all the switches are checked and this isn't a digit, then we're in full on identifier mode (as long as it's also not an underscore)
                    isIdentifier = m_current != '_'; // underscores are allowed in numeric literals, post-process their validity as necessary.
            }

            string image = builder.ToString();
            if (isIdentifier)
            {
                if (m_kws.TryGetValue(image, out var kind))
                    return new KeywordToken(location, kind);
                return new IdentifierToken(location, image);
            }
            else
            { // any of the numeric modes
                if (numberRadix is int r)
                {
                    // parse value as ulong, removing all 
                    BigInteger value = ParseBigInteger(image.Replace("_", ""), (uint)r);
                    string totalImage = $"{radixImage}${image}";

                    return new IntegerToken(location, value, totalImage);
                }
                else
                {
                    if (isFractional)
                    {
                        string wholePart = image;
                        string fractionPart = "";

                        if (image.Contains("."))
                        {
                            string[] pieces = image.Split('.');

                            wholePart = pieces[0];
                            fractionPart = pieces[1];
                        }

                        if (wholePart.Contains("e"))
                        {
                            string[] pieces = wholePart.Split('e');
                            wholePart = pieces[0];
                            fractionPart = $"0e{pieces[1]}";
                        }

                        int exponentAdd = 0;
                        if (fractionPart.Contains("e"))
                        {
                            string[] fractionPieces = fractionPart.Split('e');
                            fractionPart = fractionPieces[0].Replace("_", "");
                            exponentAdd = int.Parse(fractionPieces[1].Replace("_", ""));
                        }
                        else fractionPart = fractionPart.Replace("_", "");

                        int fractionExponent = -fractionPart.Length;
                        
                        BigInteger mantissa = BigInteger.Parse((wholePart + fractionPart).Replace("_", "")); // e.g. 123.456 -> 123456e-3
                        int exponent = exponentAdd + fractionExponent;

                        BigDecimal value = new BigDecimal(mantissa, exponent);
                        return new FloatToken(location, value, image);
                    }
                    else return new IntegerToken(location, BigInteger.Parse(image.Replace("_", "")), image);
                }
            }
        }

        private Token ReadString()
        {
            (int, int) location = (m_line, m_column);

            Advance(); // open quote `"`

            var str = new StringBuilder();
            while (!IsEoF && m_current != '"')
            {
                if (m_current == '\\')
                {
                    Advance();
                    if (IsEoF)
                        throw new NotImplementedException();

                    if (m_current == '\\') str.Append('\\');
                    else if (m_current == 'a') str.Append('\a');
                    else if (m_current == 'b') str.Append('\b');
                    else if (m_current == 'f') str.Append('\f');
                    else if (m_current == 'n') str.Append('\n');
                    else if (m_current == 'r') str.Append('\r');
                    else if (m_current == 't') str.Append('\t');
                    else if (m_current == 'v') str.Append('\v');
                    else str.Append(m_current);

                    Advance();
                }
                else
                {
                    str.Append(m_current);
                    Advance();
                }
            }

            if (m_current != '"')
                throw new NotImplementedException();

            Advance(); // close quote `"`

            string image = str.ToString();
            return new StringToken(location, Encoding.UTF8.GetBytes(image), image);
        }
    }
}
