// See https://aka.ms/new-console-template for more information

using ConsoleExtensions.Proxy;
using Demo_Net6;

var console = ConsoleProxy.Instance();
console.WriteLine("Hello, World!");

console.SetTitle("Demo");

Styling(console);

Header(console);

VanilaGreetAndAskForName();

GreetAndAskForName(console);
			
console.ReadLine(out _);


void Header(IConsoleProxy proxy)
{
    proxy.HR().WriteLine(" Fantastic 'ask your name' app").HR();
}

static void Styling(IConsoleProxy console)
{
    console
        .Style(StyleName.Ok)
        .WriteLine("Mostly things are ok")
        .Style(StyleName.Info)
        .WriteLine("But sometimes you need to be informed")
        .Style(StyleName.Warning)
        .WriteLine("Or warned")
        .Style(StyleName.Error)
        .WriteLine("Or things can go really bad")
        .ResetStyle()
        .WriteLine("But mostly everything is fine.");
}

static void GreetAndAskForName(IConsoleProxy console)
{
    console.WriteLine("Welcome user.")
        .Write("What is your name? ")
        .ReadLine(out var name)
        .WriteLine()
        .WriteLine($"Welcome {name}")
        .ReadLine(out _);
}

static void VanilaGreetAndAskForName()
{
    Console.WriteLine("Welcome user.");
    Console.WriteLine();
    Console.Write("What is your name? ");
    var name = Console.ReadLine();
    Console.WriteLine($"Welcome {name}");
    Console.ReadLine();
}