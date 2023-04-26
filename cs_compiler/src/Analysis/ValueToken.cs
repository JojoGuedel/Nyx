namespace Nyx.Analysis;

internal class ValueToken : Token
{ 
    public string value { get; }

    internal ValueToken(TokenKind kind, Location location, string value) : base(kind, location)
    {
        this.value = value;
    }
}


