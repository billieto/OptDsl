using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;

namespace OptDsl.Visitors;

public class AstPrinterVisitor : IAstVisitor
{
    private int _counter = 0;
    private readonly Stack<string> _parentStack = new();
    private readonly DotGraph _graph = new DotGraph().Directed().WithIdentifier("AST");

    public void Visit(AstTranslationUnit translationUnit)
    {
        WriteNodeAndTraverse(translationUnit.NodeName, translationUnit.Children);
    }

    public void Visit(AstOptimizationBlock optimizationBlock)
    {
        string label = $"{optimizationBlock.NodeName} ({optimizationBlock.TargetFile})";
        WriteNodeAndTraverse(label, optimizationBlock.Children);
    }

    public void Visit(AstScopeBlock scopeBlock)
    {
        string label = $"{scopeBlock.NodeName} with {scopeBlock.ScopeName} scope";
        WriteNodeAndTraverse(label, scopeBlock.Children);
    }

    public void Visit(AstOptimizationStatement optimizationStatement)
    {
        WriteNodeAndTraverse(optimizationStatement.NodeName, optimizationStatement.Children);
    }

    public void Visit(AstParameterStatement parameterStatement)
    {
        string val = parameterStatement.Value.Match(
            onInt: i => i.ToString(),
            onBool: b => b.ToString().ToLower()
        );
        string label = $"{parameterStatement.NodeName}\n{parameterStatement.ParameterName} = {val}";
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

    /// <summary>
    /// Exports the graph of the AST into a <c>.dot</c> file at <c>debug/AST.dot</c>.
    /// </summary>
    /// <param name="root">The root element of the AST to visualize.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    public static async Task SaveAsync(AstElement root)
    {
        AstPrinterVisitor visitor = new();
        root.Accept(visitor);

        Directory.CreateDirectory("debug");
        await using StreamWriter writer = new("debug/AST.dot");
        CompilationContext ctx = new(writer, new CompilationOptions());
        await visitor._graph.CompileAsync(ctx);
    }
}
