using OptDsl.Composites.Tree;

namespace OptDsl.Visitors;

public interface IAstVisitor
{
    void Visit(AstTranslationUnit translationUnit);
    void Visit(AstOptimizationBlock optimizationBlock);
    void Visit(AstOptimizationStatement optimizationStatement);
}
    
public interface IAstVisitor<out T>
{
    T Visit(AstTranslationUnit translationUnit);
    T Visit(AstOptimizationBlock optimizationBlock);
    T Visit(AstOptimizationStatement optimizationStatement);
}
