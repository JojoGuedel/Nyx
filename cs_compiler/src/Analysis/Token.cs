namespace Nyx.Analysis;

internal class Token : Node
{ 
    internal override Location location { get; }
    internal TokenKind kind { get; }

    internal Token(TokenKind kind, Location location)
    {
        this.kind = kind;
        this.location = location;
    }
}
