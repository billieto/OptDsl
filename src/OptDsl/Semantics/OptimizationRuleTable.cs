using OptDsl.Composites.Values;

namespace OptDsl.Semantics;

public static class OptimizationRuleTable
{
    private static readonly Dictionary<(string Opt, string Param), DataType> _ruleTable = new()
    {
        {("inline", "threshold"), DataType.Integer},
    };

    public static bool OptimizationExists(string opt)
        => _ruleTable.Keys.Any(k => k.Opt == opt);

    public static bool ParameterAccepted(string opt, string param)
        => _ruleTable.ContainsKey((opt, param));

    public static bool TryGetExpectedType(string opt, string param, out DataType expectedType)
        => _ruleTable.TryGetValue((opt, param), out expectedType);
}
