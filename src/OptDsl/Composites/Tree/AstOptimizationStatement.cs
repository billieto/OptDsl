using OptDsl.Composites.Core;
using OptDsl.Composites.Values;
using OptDsl.Visitors;

namespace OptDsl.Composites.Tree;

public class AstOptimizationStatement(
    string optimization,
    string parameter,
    ParameterValue value,
    int lineNumber
) : AstLeaf("Optimization Statement", lineNumber)
{
    public string Optimization { get; } = optimization;
    public string Parameter { get; } = parameter;
    public ParameterValue Value { get; } = value;

    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
