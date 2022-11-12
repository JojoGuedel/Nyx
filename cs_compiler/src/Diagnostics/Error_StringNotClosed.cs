using Utils;

namespace Diagnostics;

public class Error_StringNotClosed : ADiagnostic
{
    private char _terminator;

    public Error_StringNotClosed(TextLocation location, char terminator): base(location)
    {
        _severity = DiagnosticSeverity.Error;
        _kind = DiagnosticKind.Error_StringNotClosed;

        _terminator = terminator;
    }

    public override string GetMessage()
    {
        return "string is not closed";
    }
}