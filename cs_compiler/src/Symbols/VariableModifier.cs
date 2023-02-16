namespace Nyx.Symbols;

public class VariableModifier
{
    public bool isMutable { get; }

    public VariableModifier(bool isMutable)
    {
        this.isMutable = isMutable;
    }
}
