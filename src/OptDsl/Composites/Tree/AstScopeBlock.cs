using OptDsl.Composites.Core;
using OptDsl.Visitors;

namespace OptDsl.Composites.Tree;

public class AstScopeBlock(
    int lineNumber, 
    string scopeName
) : AstComposite("Scope Block", lineNumber)
{
    public string ScopeName { get; } = scopeName;
 
    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
