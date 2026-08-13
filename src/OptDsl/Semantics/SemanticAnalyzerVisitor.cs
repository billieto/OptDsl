using OptDsl.Composites.Grammar;
using OptDsl.Composites.Values;
using OptDsl.Visitors;

namespace OptDsl.Semantics;

public class SemanticAnalyzerVisitor : IAstVisitor
{
    public void Visit(AstTranslationUnit translationUnit)
    {
        
    }

    public void Visit(AstOptimizationBlock optimizationBlock)
    {
        throw new NotImplementedException();
    }

    public void Visit(AstOptimizationStatement optimizationStatement)
    {
        throw new NotImplementedException();
    }
}
