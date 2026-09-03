using OptDsl.Composites.Core;
using OptDsl.Composites.Values;
using OptDsl.Visitors;

namespace OptDsl.Composites.Tree;

public class AstParameterStatement(
    int lineNumber,
    string paramName,
    ParameterValue value
) : AstLeaf("Parameter Statement", lineNumber)
{
    public string ParameterName { get; } = paramName;
    public ParameterValue Value { get; } = value;

    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
