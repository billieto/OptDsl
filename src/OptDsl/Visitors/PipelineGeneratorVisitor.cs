using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;
using OptDsl.Registry;

namespace OptDsl.Visitors;

public class PipelineGeneratorVisitor : IAstVisitor
{
    private readonly List<string> _passes = [];
    private string? _targetId;

    public CodeGenResult Generate(AstElement root)
    {
        root.Accept(this);

        string fullPipeline = string.Join(",", _passes);
        return new CodeGenResult(_targetId!, fullPipeline);
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
        OptimizationRegistry.TryGetOptimization(optimizationStatement.Optimization, out var optDef);
        optDef!.TryGetParameter(optimizationStatement.Parameter, out var paramDef);

        string pass = optimizationStatement.Value.Match(
            onInt: i => $"{optDef.LlvmPassName}<{paramDef!.LlvmParamName}={i}>",
            onBool: _ => $"{optDef.LlvmPassName}<{paramDef!.LlvmParamName}>"
        );

        _passes.Add(pass);
    }
}
