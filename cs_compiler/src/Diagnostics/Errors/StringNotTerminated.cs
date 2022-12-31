namespace Nyx.Diagnostics;

using Utils;

public class StringNotTerminated : Diagnostic
{
    private char _terminator;

    public StringNotTerminated(TextLocation location, char terminator) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_StringNotTerminated, DiagnosticOrigin.LexicalAnalysis, location)
    {
        _terminator = terminator;
    }

    public override string GetMessage()
    {
        return "String was not terminated.";
    }
}