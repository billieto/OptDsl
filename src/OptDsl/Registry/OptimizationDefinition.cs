using System.Diagnostics.CodeAnalysis;

namespace OptDsl.Registry;

public class OptimizationDefinition(string name, PassScopes passScopes, params ParameterDefinition[] parameters)
{
    public string LlvmPassName { get; } = name;
    public PassScopes PassScopes { get; } = passScopes;
    private readonly Dictionary<string, ParameterDefinition> _parameters = parameters.ToDictionary(p => p.Name);

    /// <summary>
    /// Tries to get the `ParameterDefinition` associated with the parameter name given.
    /// </summary>
    /// <param name="paramName">The name of the parameter to get its definition.</param>
    /// <param name="parameter">An `out` parameter to store the parameter, if the parameter is present.</param>
    /// <returns>`true` if the parameter name is valid, otherwise `false`.</returns>
    public bool TryGetParameter(
        string paramName,
        [NotNullWhen(true)] out ParameterDefinition? parameter
    ) => _parameters.TryGetValue(paramName, out parameter);
}
