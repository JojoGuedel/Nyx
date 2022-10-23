class Program
{
    static bool running;

    public static void Main()
    {
        running = true;

        var syntax = new SyntaxDefinition();
        syntax.Define("\0", SyntaxKind.EndOfFile);

        syntax.Define("=", SyntaxKind.Equal);
        syntax.Define("==", SyntaxKind.EqualEqual);

        while (running)
        {
            Console.Write("> ");

            var input = Console.ReadLine();

            if (input is null || input.Length < 1)
                continue;

            if (input[0] == '#')
            {
                ManageEscapeCommands(input.Substring(1));
                continue;
            }

            List<SyntaxToken> tokens = new List<SyntaxToken>();
            Lexer lexer = new Lexer(input, syntax);

            while (true)
            {
                var token = lexer.NextToken();

                tokens.Add(token);

                if (token.Kind == SyntaxKind.EndOfFile)
                    break;
            }

            foreach (var token in tokens)
                Console.WriteLine(token.Kind);
        }
    }

    static void ManageEscapeCommands(string command)
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
}
