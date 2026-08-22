using OptDsl.Composites.Values;

namespace OptDsl.Registry;

public record ParameterDefinition(string Name, string LlvmParamName, DataType ExpectedType);
