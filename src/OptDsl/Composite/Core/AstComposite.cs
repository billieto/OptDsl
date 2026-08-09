namespace OptDsl.Composite.Core;

public abstract class AstComposite(string name) : AstElement(name)
{
    private readonly List<AstElement> _children = [];
    public IReadOnlyList<AstElement> Children => _children;

    protected void AddChild(AstElement child) => _children.Add(child);
}
