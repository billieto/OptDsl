using System.Diagnostics.CodeAnalysis;
using OptDsl.Composites.Values;

namespace OptDsl.Registry;

public static class OptimizationRegistry
{
    private static readonly Dictionary<string, OptimizationDefinition> _registry = new()
    {
        // {
        //     "inline",
        //     new OptimizationDefinition("inline",
        //         new ParameterDefinition("threshold", DataType.Integer)
        //     )
        // },
        {
            "simplifycfg",
            new OptimizationDefinition("simplifycfg",
            new ParameterDefinition("threshold", "bonus-inst-threshold", DataType.Integer)
            )   
        },
        {
            "constant_fold",
            new OptimizationDefinition("instcombine",
            new ParameterDefinition("max_iterations","max-iterations", DataType.Integer)
            )
        }
    };

    public static bool TryGetOptimization(string optName, [NotNullWhen(true)] out OptimizationDefinition? optimization)
        => _registry.TryGetValue(optName, out optimization);
}
