namespace Nyx.Analysis;

internal class ElseStatement : Statement
{
    internal override Location location { get; }
    public Block body { get; }

    public ElseStatement(Token @else, Token colon, Token newLine, Block body)
    {
        location = Location.Embrace(@else.location, body.location);
        this.body = body;
    }
}