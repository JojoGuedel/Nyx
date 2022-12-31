namespace Nyx.Symbols;

public class VariableSymbol : Symbol
{
    public SymbolModifiers modifiers { get; }
    public TypeSymbol type { get; }

    public VariableSymbol(string name, SymbolModifiers modifiers, TypeSymbol type) : base(name, SymbolKind.Variable)
    {
        this.modifiers = modifiers;
        this.type = type;
    }
}
