namespace Nyx.Analysis;

internal class Modifiers : Node
{
    internal override Location location { get; }
    public Token @static { get; }
    public Token mutable { get; }

    internal Modifiers(Token @static, Token mutable)
    {
        location = Location.Embrace(@static.location, mutable.location);
        this.@static = @static;
        this.mutable = mutable;
    }
}
