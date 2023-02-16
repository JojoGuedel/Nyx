namespace Nyx.Symbols;

public class FunctionModifier
{
    public bool isPublic { get; }
    public bool isStatic { get; }

    // if isStatic then canRead = false
    public bool canRead { get; }
    // if isStatic then canWrite = false
    public bool canWrite { get; }

    public FunctionModifier(bool isPublic, bool isStatic, bool canRead, bool canWrite)
    {
        this.isPublic = isPublic;
        this.isStatic = isStatic;

        this.canRead = canRead;
        this.canWrite = canWrite;
    }
}