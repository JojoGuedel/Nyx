using System.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class String : Expression
{
    public string value { get; }
    public override TypeSymbol? typeSymbol => TypeSymbol.String;

    public String(LexerNode @string) : 
        base(@string.location)
    {
        Debug.Assert(@string.value != null);

        value = @string.value;
    }
}