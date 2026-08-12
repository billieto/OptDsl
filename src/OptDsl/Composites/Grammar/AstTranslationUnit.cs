using OptDsl.Composites.Core;
using OptDsl.Visitors;

namespace OptDsl.Composites.Grammar;

public class AstTranslationUnit() : AstComposite("Translation Unit")
{
    public override void Accept(IAstVisitor visitor) => visitor.Visit(this);
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}
