namespace Nyx.Symbols;

public class TypeSymbol : Symbol
{
    public static TypeSymbol Void = new TypeSymbol("void");
    public static TypeSymbol I32 = new TypeSymbol("i32");
    public static TypeSymbol Bool = new TypeSymbol("bool");
    public static TypeSymbol String = new TypeSymbol("string");

    public TypeSymbol(string name) : 
        base(name, SymbolKind.Type) 
    {
    }
}
