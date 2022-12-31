namespace Nyx.Diagnostics;

using Utils;

public class EmptyBlock : Diagnostic
{
    public EmptyBlock(TextLocation location) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_EmptyBlock, DiagnosticOrigin.LexicalAnalyisis, location) { }

    public override string GetMessage()
    {
        return $"empty block";
    }
}