using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using OptDsl.Composites.Values;

namespace OptDsl.Registry;

public static class OptimizationRegistry
{
    private static readonly Dictionary<string, OptimizationDefinition> _registry = new()
    {
        // === Passes with Parameters ===
        {
            "simplifycfg",
            new OptimizationDefinition(
                "simplifycfg",
                new ParameterDefinition("threshold", "bonus-inst-threshold", DataType.Integer),
                new ParameterDefinition("forward_switch_cond", "forward-switch-cond", DataType.Boolean),
                new ParameterDefinition("switch_to_lookup", "switch-to-lookup", DataType.Boolean),
                new ParameterDefinition("keep_loops", "keep-loops", DataType.Boolean),
                new ParameterDefinition("hoist_common_insts", "hoist-common-insts", DataType.Boolean)
            )
        },
        {
            "constant_fold",
            new OptimizationDefinition(
                "instcombine",
                new ParameterDefinition("max_iterations", "max-iterations", DataType.Integer),
                new ParameterDefinition("verify_fixpoint", "verify-fixpoint", DataType.Boolean)
            )
        },
        {
            "loop_unroll",
            new OptimizationDefinition(
                "loop-unroll",
                new ParameterDefinition("max_iterations", "full-unroll-max", DataType.Integer),
                new ParameterDefinition("partial", "partial", DataType.Boolean),
                new ParameterDefinition("peeling", "peeling", DataType.Boolean)
            )
        },
        {
            "loop_vectorize",
            new OptimizationDefinition(
                "loop-vectorize",
                new ParameterDefinition("interleave_forced_only", "interleave-forced-only", DataType.Boolean),
                new ParameterDefinition("vectorize_forced_only", "vectorize-forced-only", DataType.Boolean)
            )
        },
        {
            "early_cse",
            new OptimizationDefinition(
                "early-cse",
                new ParameterDefinition("use_memory_ssa", "memssa", DataType.Boolean)
            )
        },
        {
            "sroa",
            new OptimizationDefinition(
                "sroa",
                new ParameterDefinition("preserve_cfg", "preserve-cfg", DataType.Boolean),
                new ParameterDefinition("modify_cfg", "modify-cfg", DataType.Boolean)
            )
        },

        // === 0-Parameter Passes (Function Level) ===
        { "memory_to_register", new OptimizationDefinition("mem2reg") },
        { "dead_code_elimination", new OptimizationDefinition("adce") },
        { "global_value_numbering", new OptimizationDefinition("gvn") },
        { "constant_propagation", new OptimizationDefinition("sccp") },
        { "tail_call_elimination", new OptimizationDefinition("tailcallelim") },
        { "instruction_simplify", new OptimizationDefinition("instsimplify") },
        { "jump_threading", new OptimizationDefinition("jump-threading") },
        { "reassociate", new OptimizationDefinition("reassociate") },

        // === 0-Parameter Passes (Loop Level) ===
        { "loop_deletion", new OptimizationDefinition("loop-deletion") },
        { "loop_idiom", new OptimizationDefinition("loop-idiom") },
        { "loop_reduce", new OptimizationDefinition("loop-reduce") },

        // === 0-Parameter Passes (Module Level) ===
        { "global_opt", new OptimizationDefinition("globalopt") },
        { "always_inline", new OptimizationDefinition("always-inline") },
        { "dead_argument_elimination", new OptimizationDefinition("deadargelim") },
        { "interprocedural_sccp", new OptimizationDefinition("ipsccp") }
    };

    public static bool TryGetOptimization(
        string optName,
        [NotNullWhen(true)] out OptimizationDefinition? optimization
    ) => _registry.TryGetValue(optName, out optimization);
}
