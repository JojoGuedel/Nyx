using Utils;

namespace Diagnostics;

public class Error_EmptyBlock : ADiagnostic
{
    public Error_EmptyBlock(TextLocation location) : base(location)
    {
        _severity = DiagnosticSeverity.Error;
        _kind = DiagnosticKind.Error_EmptyBlock;
    }

    public override string GetMessage()
    {
        return $"empty block";
    }
}