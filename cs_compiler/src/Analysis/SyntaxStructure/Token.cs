namespace Nyx.Analysis;

internal class Token : Node
{ 
    internal override Location location { get; }
    public TokenKind kind { get; }

    internal Token(TokenKind kind, Location location)
    {
        this.kind = kind;
        this.location = location;
    }

    internal Token EmptyPoint() => new Token(TokenKind.empty, location.Point());
    internal static Token Empty() => new Token(TokenKind.empty, Location.Empty());
}
