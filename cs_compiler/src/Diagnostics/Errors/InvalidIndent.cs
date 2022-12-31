namespace Nyx.Diagnostics;

using Nyx.Utils;

public class InvalidIndent : Diagnostic
{
    public InvalidIndent(TextLocation location) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_InvalidIndent, DiagnosticOrigin.LexicalAnalysis, location) { }

    public override string GetMessage()
    {
        return "Invalid indent alignment.";
    }
}