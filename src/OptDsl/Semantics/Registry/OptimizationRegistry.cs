using System.Diagnostics.CodeAnalysis;
using OptDsl.Composites.Values;

namespace OptDsl.Semantics.Registry;

public static class OptimizationRegistry
{
    private static readonly Dictionary<string, OptimizationDefinition> _registry = new()
    {
        {
            "inline",
            new OptimizationDefinition("inline",
                new ParameterDefinition("threshold", DataType.Integer)
            )
        }
    };

    public static bool TryGetOptimization(string optName, [NotNullWhen(true)] out OptimizationDefinition? optimization)
        => _registry.TryGetValue(optName, out optimization);
}
