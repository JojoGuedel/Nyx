namespace Nyx.Symbols;

public abstract class Symbol
{
    public string name { get; }
    public SymbolKind kind { get; }

    public Symbol(string name, SymbolKind kind)
    {
        this.name = name;
        this.kind = kind;
    }
}
