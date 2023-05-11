namespace Demo;

using ConsoleExtensions.Proxy;

public static class WriteExtensions
{
	/// <summary>
	/// Writes a horizontal line in the console.
	/// </summary>
	/// <param name="console">The console.</param>
	/// <returns>The used Console Proxy.</returns>
	public static IConsoleProxy Hr(this IConsoleProxy console)
	{
		console.Write(new string('-', console.WindowWidth));
		return console;
	}
}