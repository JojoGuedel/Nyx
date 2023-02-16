namespace Nyx.Symbols;

public class PropertyModifier
{
    public bool isMutable { get; }
    // if isPubMutable then isMutable = true
    public bool isPubMutable { get; }

    public bool isReadable { get; }
    // if isPubReadable then isReadable = true
    public bool isPubReadable { get; }

    public PropertyModifier(bool isMutable, bool isPubMutable, bool isReadable, bool isPubReadable)
    {
        this.isMutable = isMutable;
        this.isPubMutable = isPubMutable;

        this.isReadable = isReadable;
        this.isPubReadable = isPubReadable;
    }
}
