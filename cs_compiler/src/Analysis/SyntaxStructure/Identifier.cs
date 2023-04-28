using System.Diagnostics;

namespace Nyx.Analysis;

internal class Identifier : Expression
{
    internal override Location location => identifier.location;
    internal ValueToken identifier { get; }

    internal Identifier(ValueToken identifier)
    {
        this.identifier = identifier;
    }
}