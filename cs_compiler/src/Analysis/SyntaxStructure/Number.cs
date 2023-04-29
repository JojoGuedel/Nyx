namespace Nyx.Analysis;

internal class Number : Expression
{
    internal override Location location => number.location;
    public ValueToken number { get; }

    internal Number(ValueToken number)
    {
       this.number = number;
    }
}