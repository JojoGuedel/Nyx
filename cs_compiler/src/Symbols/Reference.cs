namespace Nyx.Symbols;

public class Reference : Type
{
    public Type type { get; }

    public Reference(Type type) : base(type.name, null)
    {
        this.type = type;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Reference type)
            return fullName == type.fullName;
        
        return false;
    }
}