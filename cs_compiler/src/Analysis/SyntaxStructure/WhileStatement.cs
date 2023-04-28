namespace Nyx.Analysis;

internal class WhileStatement : Statement
{
    internal override Location location { get; }

    public Expression condition { get; }
    public Block body { get; }

    internal WhileStatement(
        Token @while,
        Token lParen,
        Expression condition,
        Token rParen,
        Token colon,
        Token newLine,
        Block body)
    {
        location = Location.Embrace(@while.location, body.location);

        this.condition = condition;
        this.body = body;
    }
}
