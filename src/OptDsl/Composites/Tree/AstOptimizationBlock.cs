using OptDsl.Composites.Core;
using OptDsl.Visitors;

namespace OptDsl.Composites.Tree;

public class AstOptimizationBlock(
    int lineNumber,
    string targetFile
) : AstComposite("Optimization Block", lineNumber)
{
    public string TargetFile { get; } = targetFile;

    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
