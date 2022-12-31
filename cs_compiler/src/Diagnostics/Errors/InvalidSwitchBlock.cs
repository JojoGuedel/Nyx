namespace Nyx.Diagnostics;

using Nyx.Analysis;

public class InvalidSwitchBlock : Diagnostic
{
    private SyntaxNode _token;

    public InvalidSwitchBlock(SyntaxNode token) :
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_InvalidSwitchBlock, DiagnosticOrigin.SyntaxAnalysis, token.location)
    {
        _token = token;
    }

    public override string GetMessage()
    {
        return $"Unexpected token kind <{_token.kind}> in switch block";
    }
}