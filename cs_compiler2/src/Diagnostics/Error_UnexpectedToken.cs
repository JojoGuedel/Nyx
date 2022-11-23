using CodeAnalysis;

namespace Diagnostics;

public class Error_UnexpectedToken : ADiagnostic
{
    private SyntaxNode _unexpectedToken;
    private SyntaxKind[] _expectedKinds;

    public Error_UnexpectedToken(SyntaxNode unexpectedToken, SyntaxKind[] expectedKinds) : base(unexpectedToken.location)
    {
        // TODO: add diagnosticKind hint so errors can be even more distinguished
        _severity = DiagnosticSeverity.Error;
        _kind = DiagnosticKind.Error_UnexpectedToken;

        _unexpectedToken = unexpectedToken;
        _expectedKinds = expectedKinds;
    }

    public override string GetMessage()
    {
        var ret = $"unexpected token kind <{_unexpectedToken.kind}>";

        if (_expectedKinds.Length == 0)
            return ret;

        ret += $", expected <{_expectedKinds[0]}>";

        for(var i = 1; i < _expectedKinds.Length; i++)
            ret += $", <{_expectedKinds[i]}>";

        return ret;
    }
}