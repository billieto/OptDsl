namespace OptDsl.Registry;

public class SymbolTable
{
    private readonly Dictionary<string, int> _symbols = [];

    /// <summary>
    /// Tries to add the given symbol to the symbol table (Dictionary).
    /// </summary>
    /// <param name="id">The symbol to try to put.</param>
    /// <param name="lineNumber">The source line number where the symbol is declared.</param>
    /// <returns><see langword="true"/> if the symbol is added successfully, otherwise <see langword="false"/>.</returns>
    public bool TryAddSymbol(string id, int lineNumber)
        => _symbols.TryAdd(id, lineNumber);

    /// <summary>
    /// Returns the line number where the given symbol was declared.
    /// </summary>
    /// <param name="id">The name of the symbol.</param>
    /// <returns>The line number where the symbol was first declared.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when <paramref name="id"/> is not in the table.</exception>
    public int GetLineNumber(string id)
        => _symbols[id];
}
