using System.Collections;

namespace Diagnostics;

public class DiagnosticCollection
{
    private List<ADiagnostic> _diagnostics;

    public bool containsAny { get => _diagnostics.Count > 0; }
    public bool containsErrors { get => _diagnostics.Any((a) => a.severity == DiagnosticSeverity.Error); }
    public bool containsWarnings { get => _diagnostics.Any((a) => a.severity == DiagnosticSeverity.Warning); }

    public DiagnosticCollection()
    {
        _diagnostics = new List<ADiagnostic>();
    }

    public void Add(ADiagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
    }

    public IEnumerable<ADiagnostic> GetAll()
    {
        // TODO: make this better
        foreach(var diagnostic in _diagnostics)
            yield return diagnostic;
    }
}
