namespace OptDsl.Semantics;

public class SymbolTable
{
    // TODO: Later, when i will add error messages it would be nice to keep line number to say "foo was already declared in line 13"
    private readonly HashSet<string> _symbols = [];

    public bool TryAddSymbol(string id) => _symbols.Add(id);
}
