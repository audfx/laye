using System;

using Laye.Compilation;

namespace LayeC
{
    static class Program
    {
        const string TestProgram = "void main(string[] args) { var thing = \"test string\"; } \\\\ test comment\n int add(int a, int b) { return a + b; } add(16$CAFEBABE, 8$7777);";

        static void Main(string[] args)
        {
            var lexer = new Lexer();

            var tokens = lexer.GetTokens(TestProgram);
            foreach (var token in tokens)
            {
                Console.Write($"{token.GetType().Name}: ");
                if (token is IdentifierToken ident)
                    Console.WriteLine(ident.Image);
                else if (token is KeywordToken kw)
                    Console.WriteLine(kw.Kind);
                else if (token is IntegerToken i)
                    Console.WriteLine($"{i.Image} -> {i.Value}");
                else if (token is FloatToken f)
                    Console.WriteLine($"{f.Image} -> {f.Value}");
                else if (token is DelimiterToken d)
                    Console.WriteLine($"{(char)d.Kind}");
                else if (token is StringToken s)
                    Console.WriteLine($"\"{s.StringValue}\"");
                else if (token is OperatorToken o)
                    Console.WriteLine(o.Image);
                else Console.WriteLine();
            }
        }
    }
}
