using OptDsl.Composites.Core;
using OptDsl.Composites.Grammar;

namespace OptDsl.Visitors;

// TODO: See if i can replace it with https://github.com/vfrz/DotNetGraph
public class GraphPrinterVisitor : IAstVisitor
{
    private int _counter = 0;
    private const string path = "debug/AST.dot";
    private readonly StreamWriter _sw;
    private readonly Stack<string> _parentStack = new();

    public GraphPrinterVisitor()
    {
        Directory.CreateDirectory("debug");
        _sw = File.CreateText("debug/AST.dot");
        _sw.AutoFlush = true;
        _sw.WriteLine("digraph AST {");
    }

    public void Dispose()
    {
        _sw.WriteLine("}");
        _sw.Dispose();
    }

    public void Visit(AstTranslationUnit translationUnit)
    {
        WriteNodeAndTraverse(translationUnit.Name, translationUnit.Children);
    }

    public void Visit(AstOptimizationBlock optimizationBlock)
    {
        string label = $"{optimizationBlock.Name} ({optimizationBlock.TargetId})";
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

        _sw.WriteLine($"  {myId} [label=\"{label}\"];");

        if (_parentStack.Count > 0)
        {
            _sw.WriteLine($"  {_parentStack.Peek()} -> {myId};");
        }

        _parentStack.Push(myId);

        foreach (var child in children)
        {
            child.Accept(this);
        }

        _parentStack.Pop();
    }
}
