namespace Nyx.Diagnostics;

using Analysis;

public class UnexpectedToken : Diagnostic
{
    private SyntaxNode _unexpectedToken;
    private SyntaxKind[] _expectedKinds;

    public UnexpectedToken(SyntaxNode unexpectedToken, SyntaxKind[] expectedKinds) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_UnexpectedToken, DiagnosticOrigin.LexicalAnalysis, unexpectedToken.location)
    {
        // TODO: add diagnosticKind hint so errors can be even more distinguished
        _unexpectedToken = unexpectedToken;
        _expectedKinds = expectedKinds;
    }

    public override string GetMessage()
    {
        var msg = $"Unexpected token kind <{_unexpectedToken.kind}>";

        if (_expectedKinds.Length == 0)
            return msg;

        msg += $", expected <{_expectedKinds[0]}>";

        for(var i = 1; i < _expectedKinds.Length; i++)
            msg += $", <{_expectedKinds[i]}>";

        return msg + ".";
    }
}
