## Chain your method calls to make the code more readable

Go from
```c#
private static void VanilaGreetAndAskForName()
{
    Console.WriteLine("Welcome user.");
    Console.WriteLine();
    Console.Write("What is your name? ");
    var name = Console.ReadLine();
    Console.WriteLine($"Welcome {name}");
    Console.ReadLine();
}
```
To
```c#
private static void GreetAndAskForName(IConsoleProxy console)
{
    console.WriteLine("Welcome user.")
	    .Write("What is your name? ")
	    .ReadLine(out var name)
	    .WriteLine()
	    .WriteLine($"Welcome {name}")
	    .ReadLine(out _);
}
```

Not saving a lot of code I know, but it also gives you [the posibility of adding tests](UnitTestConsoleOutput.md)
