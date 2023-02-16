using Nyx.Analysis;

namespace Nyx.Symbols;

public class Variable : Symbol
{
    public override ReadonlyScope scope { get => type.scope; }
    // TODO: system to automaticly recognize type
    public Type type { get; }
    public VariableModifier modifiers { get; }

    public Variable(string name, Type type, VariableModifier modifiers) : base(name)
    {
        this.type = type;
        this.modifiers = modifiers;
    }
}
