using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Literal : Expression
{
    public string name;

    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

     public Literal(TextLocation location) : base(location)
    {
    }
}

public class Number : Expression
{

}

public class Bool : Expression
{
    public SyntaxKind value { get; }
    public override TypeSymbol? typeSymbol => TypeSymbol.Bool;

    public Bool(LexerNode @bool) : base(@bool.location)
    {
        value = @bool.kind;
    }
}