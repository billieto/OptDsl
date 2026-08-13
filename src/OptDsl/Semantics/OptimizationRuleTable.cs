using OptDsl.Composites.Values;

namespace OptDsl.Semantics;

public static class OptimizationRuleTable
{
    private static readonly Dictionary<string, string> _acceptedParams = new()
    {
        {"inline", "threshold"},
    };

    private static readonly Dictionary<string, DataType> _acceptedTypes = new()
    {
        {"threshold", DataType.Integer},
    };

    public static bool TryGetExpectedParam(string opt, out string? param)
        => _acceptedParams.TryGetValue(opt, out param);
        
    public static bool TryGetExpectedType(string param, out DataType dataType)
        => _acceptedTypes.TryGetValue(param, out dataType);
}
