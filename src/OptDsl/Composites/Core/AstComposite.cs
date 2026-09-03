namespace OptDsl.Composites.Core;

public abstract class AstComposite(
    string nodeName,
    int lineNumber
) : AstElement(nodeName, lineNumber)
{
    private readonly List<AstElement> _children = [];
    public IReadOnlyList<AstElement> Children => _children;

    public void AddChild(AstElement child) => _children.Add(child);
}
