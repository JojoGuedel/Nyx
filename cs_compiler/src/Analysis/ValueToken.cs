namespace Nyx.Analysis;

internal class ValueToken : Token
{ 
    internal string value { get; }

    internal ValueToken(TokenKind kind, Location location, string value) : base(kind, location)
    {
        this.value = value;
    }
}


