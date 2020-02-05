using System;

using Laye.Compilation;
using Laye.Syntax.Abstract;

namespace LayeC
{
    static class Program
    {
        const string TestProgram = "int bytePtrArr = 10;";

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

            Console.WriteLine();
            Console.WriteLine();

            var parser = new Parser();
            var treeHeads = parser.GetAbstractTree(tokens);

            var printer = new AbstractTreePrinter();
            foreach (var node in treeHeads)
                node.Accept(printer);
        }
    }
}
