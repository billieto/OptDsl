using OptDsl.Visitors;

namespace OptDsl.Composites.Core;

public abstract class AstElement(string nodeName, int lineNumber)
{
    public int LineNumber { get; } = lineNumber;
    public string NodeName { get; } = nodeName;

    public abstract void Accept(IAstVisitor visitor);
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}
