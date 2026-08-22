namespace OptDsl.Composites.Core;

public abstract class AstLeaf(
    string name,
    int lineNumber
) : AstElement(name, lineNumber);
