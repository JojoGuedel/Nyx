using Nyx.Analysis;

namespace Nyx.Symbols;

public class Type : Symbol
{
    public static Type Void => new Type("void", null);
    public static Type Int => new Type("int", null);
    public static Type Bool => new Type("bool", null);
    public static Type String => new Type("string", null);

    public override Scope scope { get; }

    public Type(string name, Namespace? parent) : base(name, parent)
    { 
        scope = new Scope();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Type type)
            return fullName == type.fullName;
        
        return false;
    }
}
