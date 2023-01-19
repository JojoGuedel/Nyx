using Nyx.Symbols;

namespace Nyx.Analysis;

public class Bool : Expression
{
    public bool value { get; }
    public override TypeSymbol? typeSymbol => TypeSymbol.Bool;

    public Bool(LexerNode @bool) : base(@bool.location)
    {
        value = @bool.kind == SyntaxKind.Keyword_True;
    }
}