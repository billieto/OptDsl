using System.Diagnostics.CodeAnalysis;

namespace OptDsl.Registry;

public class OptimizationDefinition(string name, params ParameterDefinition[] parameters)
{
    public string LlvmPassName { get; } = name;
    private readonly Dictionary<string, ParameterDefinition> _parameters = parameters.ToDictionary(p => p.Name);

    public bool TryGetParameter(string paramName, [NotNullWhen(true)] out ParameterDefinition? parameter)
        => _parameters.TryGetValue(paramName, out parameter);
}
