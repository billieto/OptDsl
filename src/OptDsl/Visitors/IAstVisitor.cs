using OptDsl.Composite.Core;

namespace OptDsl.Visitors;

public interface IAstVisitor
{
    void Visit(AstComposite composite);
    void Visit(AstLeaf leaf);
}
    
public interface IAstVisitor<T>
{
    T Visit(AstComposite composite);
    T Visit(AstLeaf leaf);
}
