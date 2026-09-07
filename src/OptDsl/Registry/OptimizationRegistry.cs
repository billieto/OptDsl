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
                PassScopes.Function,
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
                PassScopes.Function,
                new ParameterDefinition("max_iterations", "max-iterations", DataType.Integer),
                new ParameterDefinition("verify_fixpoint", "verify-fixpoint", DataType.Boolean)
            )
        },
        {
            "loop_unroll",
            new OptimizationDefinition(
                "loop-unroll",
                PassScopes.Loop,
                new ParameterDefinition("max_iterations", "full-unroll-max", DataType.Integer),
                new ParameterDefinition("partial", "partial", DataType.Boolean),
                new ParameterDefinition("peeling", "peeling", DataType.Boolean)
            )
        },
        {
            "loop_vectorize",
            new OptimizationDefinition(
                "loop-vectorize",
                PassScopes.Loop | PassScopes.Function,
                new ParameterDefinition("interleave_forced_only", "interleave-forced-only", DataType.Boolean),
                new ParameterDefinition("vectorize_forced_only", "vectorize-forced-only", DataType.Boolean)
            )
        },
        {
            "early_cse",
            new OptimizationDefinition(
                "early-cse",
                PassScopes.Function,
                new ParameterDefinition("use_memory_ssa", "memssa", DataType.Boolean)
            )
        },
        {
            "sroa",
            new OptimizationDefinition(
                "sroa",
                PassScopes.Function,
                new ParameterDefinition("preserve_cfg", "preserve-cfg", DataType.Boolean),
                new ParameterDefinition("modify_cfg", "modify-cfg", DataType.Boolean)
            )
        },

        // === 0-Parameter Passes (Function Level) ===
        { "memory_to_register", new OptimizationDefinition("mem2reg", PassScopes.Function) },
        { "dead_code_elimination", new OptimizationDefinition("adce", PassScopes.Function) },
        { "global_value_numbering", new OptimizationDefinition("gvn", PassScopes.Function) },
        { "constant_propagation", new OptimizationDefinition("sccp", PassScopes.Function) },
        { "tail_call_elimination", new OptimizationDefinition("tailcallelim", PassScopes.Function) },
        { "instruction_simplify", new OptimizationDefinition("instsimplify", PassScopes.Function) },
        { "jump_threading", new OptimizationDefinition("jump-threading", PassScopes.Function) },
        { "reassociate", new OptimizationDefinition("reassociate", PassScopes.Function) },

        // === 0-Parameter Passes (Loop Level) ===
        { "loop_deletion", new OptimizationDefinition("loop-deletion", PassScopes.Loop) },
        { "loop_idiom", new OptimizationDefinition("loop-idiom", PassScopes.Loop) },
        { "loop_reduce", new OptimizationDefinition("loop-reduce", PassScopes.Loop) },

        // === 0-Parameter Passes (Module Level) ===
        { "global_opt", new OptimizationDefinition("globalopt", PassScopes.Module) },
        { "always_inline", new OptimizationDefinition("always-inline", PassScopes.Module) },
        { "dead_argument_elimination", new OptimizationDefinition("deadargelim", PassScopes.Module) },
        { "interprocedural_sccp", new OptimizationDefinition("ipsccp", PassScopes.Module) }
    };

    /// <summary>
    /// Tries to get the `OptimizationDefinition` based on the optimization name given.
    /// </summary>
    /// <param name="optName">The name of the optimization to try and get its definition.</param>
    /// <param name="optimization">An `out` parameter to store the optimization, if the optimization is present.</param>
    /// <returns><see langword="true"/> if the optimization name is valid, otherwise <see langword="false"/>.</returns>
    public static bool TryGetOptimization(
        string optName,
        [NotNullWhen(true)] out OptimizationDefinition? optimization
    ) => _registry.TryGetValue(optName, out optimization);
}
