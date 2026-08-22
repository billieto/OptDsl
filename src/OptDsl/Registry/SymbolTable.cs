namespace OptDsl.Registry;

public class SymbolTable
{
    private readonly Dictionary<string, int> _symbols = [];

    // TODO: Add comments to all the methods so it can be seen in the intellisense
    public bool TryAddSymbol(string id, int lineNumber)
        => _symbols.TryAdd(id, lineNumber);

    public int GetLineNumber(string id)
        =>  _symbols[id];
}
