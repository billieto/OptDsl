namespace OptDsl.Registry;

[Flags]
public enum PassScopes
{
    None = 0,
    Module = 1,
    Loop = 2,
    Function = 4,
    All = Module | Loop | Function
}
