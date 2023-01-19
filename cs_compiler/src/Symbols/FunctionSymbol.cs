namespace Nyx.Symbols;

public class FunctionSymbol : Symbol
{
    public SymbolModifiers modifiers { get; }
    public List<VariableSymbol> arguments { get; }

    public FunctionSymbol(string name, SymbolModifiers modifiers, List<VariableSymbol> parameters, TypeSymbol returnType) : 
        base(name, SymbolKind.Function)
    {
        this.modifiers = modifiers;
        this.arguments = parameters;
    }
}
