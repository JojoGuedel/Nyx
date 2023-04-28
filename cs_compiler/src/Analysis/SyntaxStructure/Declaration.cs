namespace Nyx.Analysis;

internal class DeclarationStatement : Statement
{
    internal override Location location { get; }
    public Modifiers modifiers { get; }
    public Identifier name { get; }
    public TypeClause type { get; }
    public Expression assignment { get; }

    internal DeclarationStatement(
        Modifiers modifiers, 
        Token var, 
        Identifier name, 
        TypeClause type, 
        Token equal, 
        Expression expression, 
        Token semicolon,
        Token newLine)
    {
        location = Location.Embrace(modifiers.location, semicolon.location);

        this.modifiers = modifiers;
        this.name = name;
        this.type = type;
        this.assignment = expression;
    }
}