using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;
using OptDsl.Registry;

namespace OptDsl.Visitors;

public class PipelineGeneratorVisitor : IAstVisitor<string>
{
    private readonly List<TargetConfig> _configs = [];
    private OptimizationDefinition? _optDef = null;

    /// <summary>
    /// Generates the pass pipeline string to be given on the `opt` command.
    /// </summary>
    /// <param name="root">The root of the AST.</param>
    /// <returns>A list of <see cref="TargetConfig"/> that holds the files to apply the pipelines and the pipelines themselves.</returns>
    public IReadOnlyList<TargetConfig> Generate(AstElement root)
    {
        root.Accept(this);
        return _configs;
    }

    public string Visit(AstTranslationUnit translationUnit)
    {
        foreach (var child in translationUnit.Children)
        {
            child.Accept(this);
        }
        return "";
    }

    public string Visit(AstOptimizationBlock optimizationBlock)
    {
        List<string> stmtList = [];
        foreach (var child in optimizationBlock.Children)
        {
            stmtList.Add(child.Accept(this));
        }
        _configs.Add(
            new TargetConfig(optimizationBlock.TargetFile, string.Join(",", stmtList))
        );
        return "";
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
            return optDef!.LlvmPassName;
        }

        return $"{optDef!.LlvmPassName}<{string.Join(";", paramList)}>";
    }

    public string Visit(AstParameterStatement parameterStatement)
    {
        _optDef!.TryGetParameter(parameterStatement.ParameterName, out var paramDef);
        return parameterStatement.Value.Match(
            onInt: i => $"{paramDef!.LlvmParamName}={i}",
            onBool: b => b ? $"{paramDef!.LlvmParamName}" : ""
        );
    }
}
