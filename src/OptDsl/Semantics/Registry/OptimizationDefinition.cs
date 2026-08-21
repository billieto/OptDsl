using System.Diagnostics.CodeAnalysis;
using OptDsl.Composites.Values;

namespace OptDsl.Semantics.Registry;

public record ParameterDefinition(string Name, DataType ExpectedType);

public class OptimizationDefinition(string name, params ParameterDefinition[] parameters)
{
    public string Name { get; } = name;
    private readonly Dictionary<string, ParameterDefinition> _parameters = parameters.ToDictionary(p => p.Name);

    public bool TryGetParameter(string paramName, [NotNullWhen(true)] out ParameterDefinition? parameter)
        => _parameters.TryGetValue(paramName, out parameter);
}
