namespace Nyx.Diagnostics;

using Utils;

public class EmptyBlock : Diagnostic
{
    public EmptyBlock(TextLocation location) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_EmptyBlock, DiagnosticOrigin.LexicalAnalysis, location) { }

    public override string GetMessage()
    {
        return $"Block must contain atleast one statement but no statement was given.";
    }
}