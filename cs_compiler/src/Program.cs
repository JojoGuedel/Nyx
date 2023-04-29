using System.Collections.Immutable;
using Nyx.Analysis;
using Nyx.Diagnostics;
using Nyx.Utils;

var running = true;
var nodeWriter = new NodeWriter(Console.Out);

var input =
@"
// no public modifier
// no abstract modifier
// no extend syntax

// no templates

global func main() -> void:
    a.b.c.d.e;
    mut var a: i32 = 23123;
    print(a);";

Compile(input);
Console.ReadKey(true);

// while (running)
// {
//     Console.Write("> ");

//     var input = Console.ReadLine();

//     if (input is null)
//         input=String.Empty;

//     if (input.Length >= 1 && input[0] == '#')
//     {
//         ManageEscapeCommands(input.Substring(1));
//         continue;
//     }

//     Compile(input);
// }

void Compile(string input)
{
    var lexer = new Lexer(new StringReader(input));
    var tokens = lexer.Analyze().ToList();

    var postLexer = new PostLexer(tokens.GetEnumerator());
    tokens = postLexer.Analyze().ToList();

    nodeWriter.Write(tokens.Cast<Node>().ToImmutableArray());

    var parser = new Parser(tokens.GetEnumerator());
    var compilationUnit = parser.Analyze();

    Console.WriteLine();
    nodeWriter.Write(compilationUnit);
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