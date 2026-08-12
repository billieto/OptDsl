using OptDsl.Visitors;

namespace OptDsl.Composites.Core;

public abstract class AstElement(string name)
{
    public uint LineNumber { get; } // Used for error messages
    public string Name { get; } = name;

    public abstract void Accept(IAstVisitor visitor);
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}
