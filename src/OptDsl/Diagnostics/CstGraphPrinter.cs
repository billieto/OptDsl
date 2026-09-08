using Antlr4.Runtime.Tree;
using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;

namespace OptDsl.Diagnostics;

public class CstGraphPrinter
{
    private int _counter = 0;
    private readonly DotGraph _graph = new DotGraph().Directed().WithIdentifier("CST");

    /// <summary>
    /// Exports the graph of the CST produced by ANTLR4 into a <c>.dot</c> file at <c>debug/CST.dot</c>.
    /// </summary>
    /// <param name="tree">The root of the ANTLR4 parse tree to export.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    public async Task Export(IParseTree tree)
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
