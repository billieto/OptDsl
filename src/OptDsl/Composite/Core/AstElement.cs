using OptDsl.Visitors;

namespace OptDsl.Composite.Core;

public abstract class AstElement(string name)
{
    public uint LineNumber { get; private set; } // Used for error messages
    public string? Name { get; private set; } = name;

    public abstract void Accept(IAstVisitor visitor);
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}
