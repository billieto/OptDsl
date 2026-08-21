using System.Text;
using OptDsl.Composites.Grammar;
using OptDsl.Composites.Values;
using OptDsl.Semantics.Registry;
using OptDsl.Visitors;

namespace OptDsl.Semantics;

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

        // TODO: See if this check should be in the Semantic Analysis, maybe not.
        if (!File.Exists(file))
        {
            _errorMessage.AppendLine($"Semantic Error: Target file \"{file}\" doesn't exist.");
            HasErrors = true;
        }

        if (!_st.TryAddSymbol(file))
        {
            _errorMessage.AppendLine($"Semantic Error: Target ID \"{file}\" is already declared.");
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
            _errorMessage.AppendLine($"Semantic Error: Optimization \"{optName}\" doesn't exist.");
            HasErrors = true;
            return;
        }

        string paramName = optimizationStatement.Parameter;
        if (!optDef.TryGetParameter(paramName, out var paramDef))
        {
            _errorMessage.AppendLine($"Semantic Error: Optimization \"{optName}\" cant't have parameter \"{paramName}\".");
            HasErrors = true;
            return;
        }

        DataType dataType = optimizationStatement.Value.Match(
            onInt: _ => DataType.Integer,
            onBool: _ => DataType.Boolean
        );

        if (dataType != paramDef.ExpectedType)
        {
            _errorMessage.AppendLine($"Semantic Error: Parameter \"{paramName}\" can't have value of type \"{dataType}\".");
            HasErrors = true;
        }

        // if (!OptimizationRuleTable.OptimizationExists(opt))
        // {
        //    
        // }
        // else if (!OptimizationRuleTable.ParameterAccepted(opt, param))
        // {
        //     
        // }
        // else if (!OptimizationRuleTable.TryGetExpectedType(opt, param, out DataType expectedType) && expectedType != dataType)
        // {
        //     
        // }
    }

    public string GetErrorMessage()
    {
        return _errorMessage.ToString();
    }
}
