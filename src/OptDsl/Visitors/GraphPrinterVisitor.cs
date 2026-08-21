using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using OptDsl.Composites.Core;
using OptDsl.Composites.Grammar;

namespace OptDsl.Visitors;

public class GraphPrinterVisitor : IAstVisitor
{
    private int _counter = 0;
    private readonly Stack<string> _parentStack = new();
    private readonly DotGraph _graph = new DotGraph().Directed().WithIdentifier("AST");

    public void Visit(AstTranslationUnit translationUnit)
    {
        Directory.CreateDirectory("debug");

        WriteNodeAndTraverse(translationUnit.Name, translationUnit.Children);
    }

    public void Visit(AstOptimizationBlock optimizationBlock)
    {
        string label = $"{optimizationBlock.Name} ({optimizationBlock.TargetFile})";
        WriteNodeAndTraverse(label, optimizationBlock.Children);
    }

    public void Visit(AstOptimizationStatement optimizationStatement)
    {
        string valString = optimizationStatement.Value.Match(
            onInt: i => i.ToString(),
            onBool: b => b.ToString().ToLower()
        );

        string label = $"{optimizationStatement.Optimization} {optimizationStatement.Parameter} = {valString}";

        WriteNodeAndTraverse(label, []);
    }

    private void WriteNodeAndTraverse(string label, IEnumerable<AstElement> children)
    {
        string myId = $"node{_counter++}";

        var dotNode = new DotNode()
            .WithIdentifier(myId)
            .WithLabel(label)
            .WithShape(DotNodeShape.Box);

        _graph.Add(dotNode);

        if (_parentStack.Count > 0)
        {
            var edge = new DotEdge()
                .From(_parentStack.Peek())
                .To(myId);

            _graph.Add(edge);
        }

        _parentStack.Push(myId);

        foreach (var child in children)
        {
            child.Accept(this);
        }

        _parentStack.Pop();
    }

    public static async Task SaveAsync(AstElement root)
    {
        var visitor = new GraphPrinterVisitor();
        root.Accept(visitor);

        Directory.CreateDirectory("debug");
        await using var writer = new StreamWriter("debug/AST.dot");
        var ctx = new CompilationContext(writer, new CompilationOptions());
        await visitor._graph.CompileAsync(ctx);
    }
}
