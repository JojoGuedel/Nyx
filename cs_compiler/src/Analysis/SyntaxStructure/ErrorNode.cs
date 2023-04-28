namespace Nyx.Analysis;

internal class ErrorToken : Token
{
    public Token token { get; }
    internal ErrorToken(Token token) : base(token.kind, token.location) 
    { 
        this.token = token;
    }
}
