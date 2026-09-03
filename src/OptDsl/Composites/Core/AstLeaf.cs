namespace OptDsl.Composites.Core;

public abstract class AstLeaf(
    string nodeName,
    int lineNumber
) : AstElement(nodeName, lineNumber);
