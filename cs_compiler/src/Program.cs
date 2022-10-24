using SyntaxAnalysis;

var running = true;
var syntax = GetSyntax();

while (running)
{
    Console.Write("> ");

    var input = Console.ReadLine();

    if (input is null)
        input=String.Empty;

    if (input.Length >= 1 && input[0] == '#')
    {
        ManageEscapeCommands(input.Substring(1));
        continue;
    }

    while (true)
    {
        var token = lexer.NextToken();

        tokens.Add(token);

        if (token.Kind == SyntaxTokenKind.End)
            break;
    }

    foreach (var token in tokens)
        Console.WriteLine(token.Kind);
}

void ManageEscapeCommands(string command)
{
    switch (command)
    {
        case "exit":
            running = false;
            break;

        default:
            Console.WriteLine($"Unknown command '{command}'");
            break;
    }
}

SyntaxDefinition GetSyntax()
{
    var syntax = new SyntaxDefinition();

    syntax.Define(new SyntaxPattern("\0", SyntaxTokenKind.End));

    syntax.Define(new SyntaxPattern("+", SyntaxTokenKind.Plus));
    syntax.Define(new SyntaxPattern("-", SyntaxTokenKind.Minus));
    syntax.Define(new SyntaxPattern("*", SyntaxTokenKind.Star));
    syntax.Define(new SyntaxPattern("/", SyntaxTokenKind.Slash));
    syntax.Define(new SyntaxPattern("=", SyntaxTokenKind.Equal));
    syntax.Define(new SyntaxPattern("+=", SyntaxTokenKind.PlusEqual));
    syntax.Define(new SyntaxPattern("-=", SyntaxTokenKind.MinusEqual));
    syntax.Define(new SyntaxPattern("*=", SyntaxTokenKind.StarEqual));
    syntax.Define(new SyntaxPattern("/=", SyntaxTokenKind.SlashEqual));
    syntax.Define(new SyntaxPattern("==", SyntaxTokenKind.EqualEqual));
    syntax.Define(new SyntaxPattern("!=", SyntaxTokenKind.NotEqual));
    syntax.Define(new SyntaxPattern("<=", SyntaxTokenKind.LessEqual));
    syntax.Define(new SyntaxPattern(">=", SyntaxTokenKind.GreaterEqual));
    syntax.Define(new SyntaxPattern("<", SyntaxTokenKind.Less));
    syntax.Define(new SyntaxPattern(">", SyntaxTokenKind.Greater));

    syntax.Define(new SyntaxPattern("(", SyntaxTokenKind.LeftParen));
    syntax.Define(new SyntaxPattern(")", SyntaxTokenKind.RightParen));
    syntax.Define(new SyntaxPattern("[", SyntaxTokenKind.LeftSquare));
    syntax.Define(new SyntaxPattern("]", SyntaxTokenKind.RightSquare));
    syntax.Define(new SyntaxPattern("{", SyntaxTokenKind.LeftCurly));
    syntax.Define(new SyntaxPattern("}", SyntaxTokenKind.RightCurly));

    return syntax;
}
