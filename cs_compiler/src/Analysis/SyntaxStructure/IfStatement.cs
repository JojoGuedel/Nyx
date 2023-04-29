namespace Nyx.Analysis;

internal class IfStatement : Statement
{
    internal override Location location { get; }
    public Expression condition { get; }
    public Block body { get; }
    public Statement @else { get; }

    public IfStatement(
        Token @if,
        Token lParen, 
        Expression condition,
        Token rParen,
        Token colon,
        Token newLine, 
        Block body, 
        Statement @else)
    {
        location = Location.Embrace(@if.location, @else.location);
        this.condition = condition;
        this.body = body;
        this.@else = @else;
    }
}