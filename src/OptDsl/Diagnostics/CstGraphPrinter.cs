using Antlr4.Runtime.Tree;
using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;

namespace OptDsl.Diagnostics;

public class CstGraphPrinter(IParseTree tree)
{
    private int _counter = 0;
    private readonly DotGraph _graph = new DotGraph().Directed().WithIdentifier("CST");

    public async Task Export()
    {
        Directory.CreateDirectory("debug");

        Walk(tree, null);

        await using var writer = new StreamWriter("debug/CST.dot");
        var ctx = new CompilationContext(writer, new CompilationOptions());
        await _graph.CompileAsync(ctx);
    }

    private void Walk(IParseTree node, DotNode? parentNode)
    {
        int id = _counter++;
        string safeId = $"node{id}";
        string visualLabel = node.GetType().Name;

        if (node.ChildCount == 0) // Lexeme
        {
            visualLabel = $"\"{node.GetText()}\"";
        }

        var dotNode = new DotNode()
            .WithIdentifier(safeId)
            .WithLabel(visualLabel)
            .WithShape(DotNodeShape.Box);

        _graph.Add(dotNode);

        if (parentNode != null)
        {
            _graph.Add(new DotEdge().From(parentNode).To(dotNode));
        }

        for (int i = 0; i < node.ChildCount; i++)
        {
            Walk(node.GetChild(i), dotNode);
        }
    }
}
