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

Compile("input");
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
    // var diagnosticWriter = new DiagnosticWriter(Console.Out, input, 2);

    // var textInfo = new TextInfo(input);
    // Console.WriteLine(textInfo.ToString());

    var lexer = new Lexer(new StringReader(input));
    var tokens = lexer.Analyze().ToList();

    nodeWriter.Write(tokens.Cast<Node>().ToImmutableArray());

    var postLexer = new PostLexer(tokens.GetEnumerator());
    tokens = postLexer.Analyze().ToList();

    Console.WriteLine();
    nodeWriter.Write(tokens.Cast<Node>().ToImmutableArray());

    // var syntaxAnaylzer = new Parser(syntax, tokens);
    // var compilationUnit = syntaxAnaylzer.Analyze().ToImmutableArray();
    // nodeWriter.Write(compilationUnit);
    // Console.WriteLine();

    // diagnosticWriter.Write(lexicalAnalyzer.diagnostics);
    // diagnosticWriter.Write(postLexicalAnalyzer.diagnostics);
    // diagnosticWriter.Write(syntaxAnaylzer.diagnostics);
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