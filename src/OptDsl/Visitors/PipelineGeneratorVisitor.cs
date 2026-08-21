using OptDsl.Composites.Core;
using OptDsl.Composites.Grammar;
using OptDsl.Composites.Values;

namespace OptDsl.Visitors;

public record CodeGenResult(string TargetFile, string PassPipeline);

public class PipelineGeneratorVisitor : IAstVisitor
{
    private readonly List<string> _passes = [];
    private string? _targetId;

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
        DataType dataType = optimizationStatement.Value.Match(
            onInt: _ => DataType.Integer,
            onBool: _ => DataType.Boolean
        );

        string valStr = optimizationStatement.Value.Match(
            onInt: i => i.ToString(),
            onBool: b => b.ToString().ToLower()
        );

        if (dataType == DataType.Integer)
        {
            _passes.Add($"{optimizationStatement.Optimization}<{optimizationStatement.Parameter}={valStr}>");
        }
        else
        {
            _passes.Add($"{optimizationStatement.Optimization}<{optimizationStatement.Parameter}>");
        }
    }
}
