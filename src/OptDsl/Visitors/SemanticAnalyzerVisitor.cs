using System.Text;
using OptDsl.Composites.Tree;
using OptDsl.Composites.Values;
using OptDsl.Registry;

namespace OptDsl.Visitors;

public class SemanticAnalyzerVisitor : IAstVisitor
{
    private readonly SymbolTable _st = new();
    private readonly StringBuilder _errorMessage = new();
    public bool HasErrors { get; private set; } = false;

    public void Visit(AstTranslationUnit translationUnit)
    {
        foreach (var child in translationUnit.Children)
        {
            child.Accept(this);
        }
    }

    public void Visit(AstOptimizationBlock optimizationBlock)
    {
        string file = optimizationBlock.TargetFile;

        if (!File.Exists(file))
        {
            _errorMessage.AppendLine(
                $"[Line {optimizationBlock.LineNumber}] Semantic Error: Target file \"{file}\" doesn't exist."
            );
            HasErrors = true;
        }

        if (!_st.TryAddSymbol(file, optimizationBlock.LineNumber))
        {
            int line = _st.GetLineNumber(file);
            _errorMessage.AppendLine(
                $"[Line {optimizationBlock.LineNumber}] Semantic Error: Target ID \"{file}\" is already declared in Line {line}."
            );
            HasErrors = true;
        }

        foreach (var child in optimizationBlock.Children)
        {
            child.Accept(this);
        }
    }

    public void Visit(AstOptimizationStatement optimizationStatement)
    {
        string optName = optimizationStatement.Optimization;
        if (!OptimizationRegistry.TryGetOptimization(optName, out var optDef))
        {
            _errorMessage.AppendLine(
                $"[Line {optimizationStatement.LineNumber}] Semantic Error: Optimization \"{optName}\" doesn't exist."
            );
            HasErrors = true;
            return;
        }

        string paramName = optimizationStatement.Parameter;
        if (!optDef.TryGetParameter(paramName, out var paramDef))
        {
            _errorMessage.AppendLine(
                $"[Line {optimizationStatement.LineNumber}] Semantic Error: Optimization \"{optName}\" cant't have parameter \"{paramName}\"."
            );
            HasErrors = true;
            return;
        }

        DataType dataType = optimizationStatement.Value.Match(
            onInt: _ => DataType.Integer,
            onBool: _ => DataType.Boolean
        );

        if (dataType != paramDef.ExpectedType)
        {
            _errorMessage.AppendLine(
                $"[Line {optimizationStatement.LineNumber}] Semantic Error: Parameter \"{paramName}\" can't have value of type \"{dataType}\"."
            );
            HasErrors = true;
        }
    }

    public string GetErrorMessage()
    {
        return _errorMessage.ToString();
    }
}
