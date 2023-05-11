## Use extension methods to add new features to the console

```c#
var console = ConsoleProxy.Instance();

console.HR()
    .WriteLine(" Fantastic 'ask your name' app")
    .HR();

public static class WriteExtensions
{
	public static IConsoleProxy HR(this IConsoleProxy console)
	{
		console.Write(new string('-', console.WindowWidth));
		return console;
	}
}
```