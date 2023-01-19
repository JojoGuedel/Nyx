using System.Diagnostics;
using Nyx.Symbols;

namespace Nyx.Analysis;

public class Number : Expression
{
    public string value { get; }
    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public Number(LexerNode number) : base(number.location)
    {
        Debug.Assert(number.value != null);
        
        value = number.value;
    }
}