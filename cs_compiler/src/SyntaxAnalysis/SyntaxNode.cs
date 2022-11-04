using Utils;

namespace SyntaxAnalysis;

public class SyntaxNode
{
    public SyntaxKind kind { get; }
    public TextLocation location { get; }
    public bool valid { get; }

    public List<SyntaxNode> children { get; }

    public SyntaxNode(SyntaxKind kind, TextLocation location, bool valid = true)
    {
        this.kind = kind;
        this.location = location;
        this.valid = valid;
        children = new List<SyntaxNode>();
    }

    public SyntaxNode(SyntaxKind kind, params SyntaxNode[] children): this(kind, children.ToList(), true) {}
    public SyntaxNode(SyntaxKind kind, bool valid, params SyntaxNode[] children): this(kind, children.ToList(), valid) {}
    public SyntaxNode(SyntaxKind kind, List<SyntaxNode> children, bool valid = true)
    {
        if (children.Count < 1)
            throw new ArgumentException("Syntax must have more than 0 childrens", nameof(children));

        this.kind = kind;
        this.location = TextLocation.Embrace(children[0].location, children.Last().location);
        this.valid = valid;
        this.children = children;
    }

    public static SyntaxNode EmptySyntax(int pos, bool valid = true)
    {
        var location = new TextLocation(pos, 0);
        var syntaxNode = new SyntaxNode(SyntaxKind.Syntax_Empty, location, valid);
        return syntaxNode;
    }
}
