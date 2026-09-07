using System.Diagnostics;

namespace OptDsl.Composites.Values;

public readonly struct ParameterValue
{
    private readonly DataType _type;
    private readonly int _intValue;
    private readonly bool _boolValue;

    public ParameterValue(int value)
    {
        _type = DataType.Integer;
        _intValue = value;
        _boolValue = default;
    }

    public ParameterValue(bool value)
    {
        _type = DataType.Boolean;
        _intValue = default;
        _boolValue = value;
    }

    /// <summary>
    /// Pattern-matches on the stored value's type and executes the corresponding delegate.
    /// This avoids messy null checks and flag fields by having the caller provide
    /// 2 functions, or lambdas, to be executed at runtime based on
    /// the value assigned in the parameter in the DSL.
    /// </summary>
    /// <param name="onInt">A function, or a lambda, to be executed if the type of the value stored is <see langword="int"/>.</param>
    /// <param name="onBool">A function, or a lambda, to be executed if the type of the value stored is <see langword="bool"/>.</param>
    /// <returns>The result produced by the matching delegate of type <typeparamref name="TResult"/>.</returns>
    public TResult Match<TResult>(Func<int, TResult> onInt, Func<bool, TResult> onBool)
    {
        return _type switch
        {
            DataType.Integer => onInt(_intValue),
            DataType.Boolean => onBool(_boolValue),
            _ => throw new UnreachableException()
        };
    }

    public static implicit operator ParameterValue(int value) => new(value);
    public static implicit operator ParameterValue(bool value) => new(value);
}
