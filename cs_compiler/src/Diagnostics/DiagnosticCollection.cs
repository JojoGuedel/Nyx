namespace Nyx.Diagnostics;

public class DiagnosticCollection
{
    private List<Diagnostic> _diagnostics;

    public bool containsAny { get => _diagnostics.Count > 0; }
    public bool containsErrors { get => _diagnostics.Any((a) => a.severity == DiagnosticSeverity.Error); }
    public bool containsWarnings { get => _diagnostics.Any((a) => a.severity == DiagnosticSeverity.Warning); }

    public DiagnosticCollection()
    {
        _diagnostics = new List<Diagnostic>();
    }

    public void Add(Diagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
    }

    public IEnumerable<Diagnostic> GetAll()
    {
        // TODO: make this better
        foreach(var diagnostic in _diagnostics)
            yield return diagnostic;
    }
}
