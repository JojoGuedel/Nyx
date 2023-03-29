using System.Collections.Immutable;
using Nyx.Analysis;

namespace Nyx.Symbols;

public class GlobalFunction : Symbol
{
    // TODO: generate scope: function.invoke
    public override ReadonlyScope? scope { get => null; }
    public ImmutableArray<Parameter> parameters { get; }
    // TODO: system to automaticly recognize returntype
    public Type returnType { get; }

    public GlobalFunction(string name, ImmutableArray<Parameter> parameters, Type returnType, Namespace? parent) : base(name, parent)
    {
        this.parameters = parameters;
        this.returnType = returnType;
    }
}
