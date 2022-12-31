namespace Nyx.Diagnostics;

using Utils;

public abstract class Diagnostic
{
    public DiagnosticSeverity severity { get; }
    public DiagnosticKind kind { get; }
    public DiagnosticOrigin origin { get; }
    public TextLocation location { get; }

    public Diagnostic(DiagnosticSeverity sevierity, DiagnosticKind kind, DiagnosticOrigin origin, TextLocation location)
    {
        this.severity = severity;
        this.kind = kind;
        this.origin = origin;
        this.location = location;
    }

    public abstract string GetMessage();
    // public abstract string GetExample();
    // public abstract string GetSolution();
    // public abstract string GetHelp();
}