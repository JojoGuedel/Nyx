using System.Collections.Immutable;
using Nyx.Analysis;
using Nyx.Diagnostics;
using Nyx.Utils;

var running = true;
var syntax = SyntaxInfo.Default();
var nodeWriter = new NodeWriter(Console.Out);

var input =
@"
// no public modifier
// no abstract modifier
// no extend syntax

// no templates

static func add(var a: i32, var b: i32) -> i32:
    return a + b;

static func main() -> void:
    mut var a: i32 = 23123;
    print(a);";

Compile(input);
Console.ReadKey(true);

// while (running)
// {
//     Console.Write("> ");

//     var input = Console.ReadLine();
//     input =
// @"
// func b(c,):
//         // var a = 10;
//     if true:
//         pass;
//     else if true:
//         pass;
//     else:
//         pass;

// func d():
//     pass;";

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
    var diagnosticWriter = new DiagnosticWriter(Console.Out, input, 2);

    var textInfo = new TextInfo(input);
    Console.WriteLine(textInfo.ToString());

    var lexicalAnalyzer = new LexicalAnalyzer(syntax, input);
    var tokens = lexicalAnalyzer.GetAll().ToList();

    var postLexicalAnalyzer = new PostlexicalAnalyzer(syntax, tokens);
    tokens = postLexicalAnalyzer.GetAll().ToList();

    var syntaxAnaylzer = new SyntaxAnalyzer(syntax, tokens);
    var compilationUnit = syntaxAnaylzer.GetAll().ToImmutableArray();
    nodeWriter.Write(compilationUnit);
    Console.WriteLine();

    diagnosticWriter.Write(lexicalAnalyzer.diagnostics);
    diagnosticWriter.Write(postLexicalAnalyzer.diagnostics);
    diagnosticWriter.Write(syntaxAnaylzer.diagnostics);
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