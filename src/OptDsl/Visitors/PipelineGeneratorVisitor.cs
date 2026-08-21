using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;

namespace OptDsl.Visitors;

public class PipelineGeneratorVisitor : IAstVisitor
{
    private readonly List<string> _passes = [];
    private string _targetId;

    public CodeGenResult Generate(AstElement root)
    {
        root.Accept(this);

        string fullPipeline = string.Join(",", _passes);
        return new CodeGenResult(_targetId, fullPipeline);
        // TODO: If there is not Optimization Block what happens? (cant pass syntax, but have to check.)
    }

    public void Visit(AstTranslationUnit translationUnit)
    {
        foreach (var child in translationUnit.Children)
        {
            child.Accept(this);
        }
    }

    public void Visit(AstOptimizationBlock optimizationBlock)
    {
        _targetId = optimizationBlock.TargetFile;

        foreach (var child in optimizationBlock.Children)
        {
            child.Accept(this);
        }
    }

    public void Visit(AstOptimizationStatement optimizationStatement)
    {
        string pass = optimizationStatement.Value.Match(
            onInt: i => $"{optimizationStatement.Optimization}<{optimizationStatement.Parameter}={i}>",
            onBool: _ => $"{optimizationStatement.Optimization}<{optimizationStatement.Parameter}>"
        );
        _passes.Add(pass);
    }
}
