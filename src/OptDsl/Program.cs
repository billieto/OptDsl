using Antlr4.Runtime;
using OptDsl.Diagnostics;
using OptDsl.Drivers;
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

        // Building the CST graph
        CstGraphPrinter cgp = new();
        await cgp.Export(cstRoot);

        // Building the AST
        CstToAstVisitor cstToAstVisitor = new();
        var astRoot = cstToAstVisitor.Visit(cstRoot);

        // Building the AST graph
        AstPrinterVisitor astPrinter = new();
        await astPrinter.SaveAsync(astRoot);

        // Semantic Analysis
        SemanticAnalyzerVisitor semanticAnalyzer = new();
        string? errors = semanticAnalyzer.Analyze(astRoot);
        if (errors != null)
        {
            Console.WriteLine(errors);
            return;
        }

        // Code Generator
        PipelineGeneratorVisitor pipelineGenerator = new();
        var targetConfigs = pipelineGenerator.Generate(astRoot);

        if (targetConfigs.Count == 0)
        {
            return;
        }

        // Driver Invocation
        LlvmDriver driver = new();
        bool failed = driver.Execute(targetConfigs);
        if (failed)
        {
            Console.WriteLine("Compilation failed due to toolchain errors.");
            return;
        }
    }
}
