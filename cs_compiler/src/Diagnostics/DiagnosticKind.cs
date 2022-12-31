namespace Nyx.Diagnostics;

public enum DiagnosticKind
{
    Error_UnexpectedToken,
    Error_StringNotTerminated,
    Error_InvalidStatement,
    Error_EmptyBlock,
    Error_TooManyIndents,
    Error_InvalidIndent,
    Error_InvalidSwitchBlock,
    Error_EmptySwitch,
}