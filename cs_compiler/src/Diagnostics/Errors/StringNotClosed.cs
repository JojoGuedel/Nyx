namespace Nyx.Diagnostics;

using Utils;

public class StringNotClosed : Diagnostic
{
    private char _terminator;

    public StringNotClosed(TextLocation location, char terminator) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_StringNotClosed, DiagnosticOrigin.LexicalAnalyisis, location)
    {
        _terminator = terminator;
    }

    public override string GetMessage()
    {
        return "string is not closed";
    }
}