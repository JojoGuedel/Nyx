using Nyx.Analysis;

namespace Nyx.Symbols;

public class Property : Symbol
{
    public override ReadonlyScope scope { get => type.scope; }
    // TODO: system to automaticly recognize type
    public Type type { get; }
    public PropertyModifier modifiers { get; }

    public Property(string name, Type type, PropertyModifier modifiers, Type parent) : base(name, parent)
    {
        this.type = type;
        this.modifiers = modifiers;
    }
}
