using Antlr4.Runtime;
using OptDsl.Extra;
using OptDsl.Visitors;

namespace OptDsl;

static class Program
{
    static async Task Main(string[] args)
    {
        // 1. Check if the user actually provided an argument
        if (args.Length == 0)
        {
            Console.WriteLine("Error: Please provide an input string or a file path.");
            Console.WriteLine("Usage: dotnet run -- \"*.optd\"");
            return;
        }

        string inputArg = args[0];
        ICharStream stream;

        // 2. Check if the argument is an existing file. If so, read from the file.
        // Otherwise, treat the argument itself as the input string.
        if (File.Exists(inputArg))
        {
            Console.WriteLine($"Reading from file: '{inputArg}'\n");
            stream = CharStreams.fromPath(inputArg);
        }
        else
        {
            Console.WriteLine($"Reading from string: '{inputArg}'\n");
            stream = CharStreams.fromString(inputArg);
        }

        // 3. Standard ANTLR pipeline
        OptDslLexer lexer = new(stream);
        CommonTokenStream tokens = new(lexer);
        OptDslParser parser = new(tokens);

        // 4. Parse and print the tree
        var cstRoot = parser.translation_unit();
        Console.WriteLine("Concrete Syntax Tree:");
        Console.WriteLine(cstRoot.ToStringTree(parser));

        CstToAstVisitor cstToAstVisitor = new();
        var astRoot = cstToAstVisitor.Visit(cstRoot);
        
        CstGraphPrinter cgp = new(cstRoot);
        await cgp.Export();

        GraphPrinterVisitor graphPrinterVisitor = new();
        astRoot.Accept(graphPrinterVisitor);
        graphPrinterVisitor.Dispose();
    }
}
