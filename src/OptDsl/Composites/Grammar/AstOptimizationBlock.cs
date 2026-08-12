using OptDsl.Composites.Core;
using OptDsl.Visitors;

namespace OptDsl.Composites.Grammar;

public class AstOptimizationBlock(string targetId) : AstComposite("Optimization Block")
{
    public string TargetId { get; } = targetId;

    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
