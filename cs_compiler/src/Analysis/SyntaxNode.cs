namespace Nyx.Analysis;

using Utils;

public class SyntaxNode
{
    public SyntaxKind kind { get; }
    public TextLocation location { get; }
    public string? value { get; }
    // TODO: remove this if it isn't useful later
    public bool isValid { get; }

    public List<SyntaxNode> children { get; }

    public SyntaxNode(SyntaxKind kind, TextLocation location, string? value = null, bool isValid = true)
    {
        this.kind = kind;
        this.location = location;
        this.value = value;
        this.isValid = isValid;
        children = new List<SyntaxNode>();
    }

    public SyntaxNode(SyntaxKind kind, params SyntaxNode[] children) : 
        this(kind, children.ToList(), true)
    { }

    public SyntaxNode(SyntaxKind kind, bool valid, params SyntaxNode[] children) : 
        this (kind, children.ToList(), valid)
    { }

    public SyntaxNode(SyntaxKind kind, List<SyntaxNode> children, bool valid = true)
    {
        if (children.Count < 1)
            throw new ArgumentException("Syntax must have more than 0 childrens", nameof(children));

        this.kind = kind;
        location = TextLocation.Embrace(children[0].location, children.Last().location);
        this.isValid = valid;
        this.children = children;
    }

    public static SyntaxNode EmptySyntax(int pos, bool valid = true)
    {
        var location = new TextLocation(pos, 0);
        var syntaxNode = new SyntaxNode(SyntaxKind.Syntax_Empty, location, isValid: valid);
        return syntaxNode;
    }

    public bool isEmptySyntax()
    {
        return kind == SyntaxKind.Syntax_Empty;
    }
}
