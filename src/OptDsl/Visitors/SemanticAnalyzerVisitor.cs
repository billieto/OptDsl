using System.Text;
using OptDsl.Composites.Tree;
using OptDsl.Composites.Values;
using OptDsl.Registry;

namespace OptDsl.Visitors;

public class SemanticAnalyzerVisitor : IAstVisitor
{
    private readonly SymbolTable _st = new();
    private readonly StringBuilder _errorMessage = new();
    private OptimizationDefinition? _optDef = null;
    private string? _optName = null;
    private readonly Stack<PassScopes> _scopeStack = [];
    public bool HasErrors => _errorMessage.Length > 0;

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
        }

        if (!_st.TryAddSymbol(file, optimizationBlock.LineNumber))
        {
            int line = _st.GetLineNumber(file);
            _errorMessage.AppendLine(
                $"[Line {optimizationBlock.LineNumber}] Semantic Error: Target ID \"{file}\" is already declared in Line {line}."
            );
        }

        foreach (var child in optimizationBlock.Children)
        {
            child.Accept(this);
        }
    }

    public void Visit(AstScopeBlock scopeBlock)
    {
        Enum.TryParse<PassScopes>(scopeBlock.ScopeName, true, out var scopeFlag);
        _scopeStack.Push(scopeFlag);
        foreach (var child in scopeBlock.Children)
        {
            child.Accept(this);
        }
        _scopeStack.Pop();
    }

    public void Visit(AstOptimizationStatement optimizationStatement)
    {
        _optName = optimizationStatement.OptimizationName;
        if (!OptimizationRegistry.TryGetOptimization(_optName, out _optDef))
        {
            _errorMessage.AppendLine(
                $"[Line {optimizationStatement.LineNumber}] Semantic Error: Optimization \"{_optName}\" doesn't exist."
            );
            return;
        }

        if (_scopeStack.Count > 0 && !_optDef.PassScopes.HasFlag(_scopeStack.Peek()))
        {
            _errorMessage.AppendLine(
                $"[Line {optimizationStatement.LineNumber}] Semantic Error: Optimization \"{_optName}\" can't be in the scope \"{_scopeStack.Peek()}\" (allowed in {_optDef.PassScopes})."
            );
        }

        foreach (var child in optimizationStatement.Children)
        {
            child.Accept(this);
        }

        _optName = string.Empty;
        _optDef = null;
    }

    public void Visit(AstParameterStatement parameterStatement)
    {
        string paramName = parameterStatement.ParameterName;
        if (!_optDef!.TryGetParameter(paramName, out var paramDef))
        {
            _errorMessage.AppendLine(
                $"[Line {parameterStatement.LineNumber}] Semantic Error: Optimization \"{_optName}\" can't have parameter \"{paramName}\"."
            );
            return;
        }

        DataType dataType = parameterStatement.Value.Match(
            onInt: _ => DataType.Integer,
            onBool: _ => DataType.Boolean
        );

        if (dataType != paramDef.ExpectedType)
        {
            _errorMessage.AppendLine(
                $"[Line {parameterStatement.LineNumber}] Semantic Error: Parameter \"{paramName}\" can't have value of type \"{dataType}\"."
            );
        }
    }

    /// <summary>
    /// Returns the errors that happened during the semantic analysis.
    /// </summary>
    /// <returns>A string with the errors found.</returns>
    public string GetErrorMessage() => _errorMessage.ToString();
}
