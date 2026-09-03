using OptDsl.Composites.Core;
using OptDsl.Visitors;

namespace OptDsl.Composites.Tree;

public class AstOptimizationStatement(
    int lineNumber,
    string optimizationName
) : AstComposite("Optimization Statement", lineNumber)
{
    public string OptimizationName { get; } = optimizationName;

    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
