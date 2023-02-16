using Nyx.Analysis;

namespace Nyx.Symbols;

public class Namespace : Symbol
{
    public override Scope scope { get; }

    public Namespace(string name) : base(name)
    {
        scope = new Scope();
    }
}
