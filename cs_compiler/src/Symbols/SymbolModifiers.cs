namespace Nyx.Symbols;

public class SymbolModifiers
{
    public bool isStatic { get; }
    public bool isMutalbe { get; }

    public SymbolModifiers(bool isStatic, bool isMutalbe)
    {
        this.isStatic = isStatic;
        this.isMutalbe = isMutalbe;
    }
}
