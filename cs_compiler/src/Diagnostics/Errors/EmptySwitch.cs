namespace Nyx.Diagnostics;

using Utils;

public class EmptySwitch : Diagnostic
{
    public EmptySwitch(TextLocation location) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_EmptySwitch, DiagnosticOrigin.LexicalAnalysis, location) { }

    public override string GetMessage()
    {
        return $"Switch block must contain atleast one case but no case was given.";
    }
}