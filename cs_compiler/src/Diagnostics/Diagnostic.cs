namespace Nyx.Diagnostics;

using Utils;

public abstract class Diagnostic
{
    public DiagnosticSeverity severity { get => _severity; }
    protected DiagnosticSeverity _severity;

    public DiagnosticKind kind { get => _kind; }
    protected DiagnosticKind _kind;

    public TextLocation location { get => _location; }
    protected TextLocation _location;

    public Diagnostic(TextLocation location)
    {
        _location = location;
    }

    public abstract string GetMessage();
    // public abstract string GetExample();
    // public abstract string GetSolution();
    // public abstract string GetHelp();
}
