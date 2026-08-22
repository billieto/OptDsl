using OptDsl.Visitors;

namespace OptDsl.Composites.Core;

public abstract class AstElement(string name, int lineNumber)
{
    public int LineNumber { get; } = lineNumber;
    public string Name { get; } = name;

    public abstract void Accept(IAstVisitor visitor);
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}
