using OptDsl.Composites.Tree;

namespace OptDsl.Visitors;

public interface IAstVisitor
{
    void Visit(AstTranslationUnit translationUnit);
    void Visit(AstOptimizationBlock optimizationBlock);
    void Visit(AstScopeBlock scopeBlock);
    void Visit(AstOptimizationStatement optimizationStatement);
    void Visit(AstParameterStatement parameterStatement);
}
    
public interface IAstVisitor<out T>
{
    T Visit(AstTranslationUnit translationUnit);
    T Visit(AstOptimizationBlock optimizationBlock);
    T Visit(AstScopeBlock scopeBlock);
    T Visit(AstOptimizationStatement optimizationStatement);
    T Visit(AstParameterStatement parameterStatement);
}
