using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;
using OptDsl.Registry;

namespace OptDsl.Visitors;

public class PipelineGeneratorVisitor : IAstVisitor<string>
{
    private string? _targetId;
    private OptimizationDefinition? _optDef = null;

    public CodeGenResult Generate(AstElement root)
    {
        string fullPipeline = root.Accept(this);
        return new CodeGenResult(_targetId!, fullPipeline);
    }

    public string Visit(AstTranslationUnit translationUnit)
    {
        List<string> blocks = [];
        foreach (var child in translationUnit.Children)
        {
            blocks.Add(child.Accept(this));
        }
        return string.Join(",", blocks);
    }

    public string Visit(AstOptimizationBlock optimizationBlock)
    {
        _targetId = optimizationBlock.TargetFile;

        List<string> stmtList = [];
        foreach (var child in optimizationBlock.Children)
        {
            stmtList.Add(child.Accept(this));
        }
        return string.Join(",", stmtList);
    }

    public string Visit(AstScopeBlock scopeBlock)
    {
        List<string> stmtList = [];
        foreach (var child in scopeBlock.Children)
        {
            stmtList.Add(child.Accept(this));
        }
        return $"{scopeBlock.ScopeName}({string.Join(",", stmtList)})";
    }

    public string Visit(AstOptimizationStatement optimizationStatement)
    {
        OptimizationRegistry.TryGetOptimization(optimizationStatement.OptimizationName, out var optDef);

        List<string> paramList = [];
        _optDef = optDef;
        foreach (var param in optimizationStatement.Children)
        {
            string paramStr = param.Accept(this);
            if (!string.IsNullOrEmpty(paramStr))
            {
                paramList.Add(paramStr);
            }
        }
        _optDef = null;

        if (paramList.Count == 0)
        {
            return optDef.LlvmPassName;
        }

        return $"{optDef.LlvmPassName}<{string.Join(";", paramList)}>";
    }

    public string Visit(AstParameterStatement parameterStatement)
    {
        _optDef!.TryGetParameter(parameterStatement.ParameterName, out var paramDef);
        return parameterStatement.Value.Match(
            onInt: i => $"{paramDef.LlvmParamName}={i}",
            onBool: b => b ? $"{paramDef.LlvmParamName}" : ""
        );
    }
}
