using System.Collections.Immutable;
using Nyx.Analysis;

namespace Nyx.Symbols;

public class Function : Symbol
{
    // TODO: generate scope: function.invoke
    public override ReadonlyScope? scope { get => null; }
    public ImmutableArray<Parameter> parameters { get; }
    // TODO: system to automaticly recognize returntype
    public Type returnType { get; }
    public FunctionModifier modifiers { get; }

    public Function(string name, ImmutableArray<Parameter> parameters, Type returnType, FunctionModifier modifiers) : base(name)
    {
        this.parameters = parameters;
        this.returnType = returnType;
        this.modifiers = modifiers;
    }
}
