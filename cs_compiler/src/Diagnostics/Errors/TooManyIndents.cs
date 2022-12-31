namespace Nyx.Diagnostics;

using Nyx.Utils;

public class TooManyIndents : Diagnostic
{
    public TooManyIndents(TextLocation location) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_TooManyIndents, DiagnosticOrigin.PostlexicalAnalysis, location) { }

    public override string GetMessage()
    {
        return "Too many indents.";
    }
}