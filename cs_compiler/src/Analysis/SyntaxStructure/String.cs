namespace Nyx.Analysis;

internal class String : Expression
{
    internal override Location location => @string.location;
    public ValueToken @string { get; }

    public String(ValueToken @string)
    {
        this.@string = @string;
    }
}