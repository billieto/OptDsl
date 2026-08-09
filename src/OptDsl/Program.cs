using Antlr4.Runtime;

namespace OptDsl;

static class Program
{
    static void Main(string[] args)
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
        var tree = parser.translation_unit();
        Console.WriteLine("Syntax Tree:");
        Console.WriteLine(tree.ToStringTree(parser));
    }
}
