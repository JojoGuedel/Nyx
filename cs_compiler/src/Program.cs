using Nyx.CodeAnalysis;

var running = true;
var syntax = SyntaxDefinition.Default();
var syntaxNodeWriter = new SytnaxNodeWriter(Console.Out);

while (running)
{
    Console.Write("> ");

    var input = Console.ReadLine();
    input = 
@"
static struct a:
    pass;

func b(c,):
    pass;

func d():
    pass;";

    if (input is null)
        input=String.Empty;

    if (input.Length >= 1 && input[0] == '#')
    {
        ManageEscapeCommands(input.Substring(1));
        continue;
    }

    var lexicalAnalyzer = new LexicalAnalyzer(syntax, input);
    var tokens = lexicalAnalyzer.GetAll().ToList();
    syntaxNodeWriter.Write(tokens);
    Console.WriteLine();

    var postlexicalAnalyzer = new PostlexicalAnalyzer(syntax, tokens);
    tokens = postlexicalAnalyzer.GetAll().ToList();
    syntaxNodeWriter.Write(tokens);
    Console.WriteLine();

    var syntaxAnaylzer = new SyntaxAnalyzer(syntax, tokens);
    var compilationUnit = syntaxAnaylzer.GetAll().ToList();
    syntaxNodeWriter.Write(compilationUnit);
    Console.WriteLine();
}

void ManageEscapeCommands(string command)
{
    switch (command)
    {
        case "exit":
            running = false;
            break;

        case "clear":
        case "cls":
            Console.Clear();
            break;

        default:
            Console.WriteLine($"Unknown command '{command}'");
            break;
    }
}