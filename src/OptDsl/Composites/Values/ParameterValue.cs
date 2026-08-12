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
