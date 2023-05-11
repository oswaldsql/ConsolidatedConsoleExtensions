namespace ConsoleExtensions.Proxy;

using System;

/// <summary>
/// Class ConsoleStyle.
/// </summary>
public class ConsoleStyle
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConsoleStyle"/> class.
	/// </summary>
	/// <param name="name">The name of the style.</param>
	/// <param name="foreground">The foreground color of the console.</param>
	/// <param name="background">The background color of the console..</param>
	public ConsoleStyle(string name, ConsoleColor? foreground = null, ConsoleColor? background = null)
	{
		this.Name = name;
		this.Foreground = foreground;
		this.Background = background;
	}

	/// <summary>
	/// Gets the the default style used by the console.
	/// </summary>
	public static ConsoleStyle Default { get; } = new("Default", ConsoleColor.Gray, ConsoleColor.Black);

	/// <summary>
	/// Gets the style used for displaying errors.
	/// </summary>
	public static ConsoleStyle Error { get; } = new("Error", ConsoleColor.Red, ConsoleColor.Black);

	/// <summary>
	/// Gets the style used for displaying informational content.
	/// </summary>
	public static ConsoleStyle Info { get; } = new("Info", ConsoleColor.Blue, ConsoleColor.Gray);

	/// <summary>
	/// Gets the style used for displaying OK information.
	/// </summary>
	public static ConsoleStyle Ok { get; } = new("Ok", ConsoleColor.Green, ConsoleColor.Black);

	/// <summary>
	/// Gets the style used for warnings.
	/// </summary>
	public static ConsoleStyle Warning { get; } = new("Warning", ConsoleColor.Yellow, ConsoleColor.Black);

	/// <summary>
	/// Gets the background color of the console.
	/// </summary>
	/// <value>The background.</value>
	public ConsoleColor? Background { get; }

	/// <summary>
	/// Gets the foreground color of the console.
	/// </summary>
	/// <value>The foreground.</value>
	public ConsoleColor? Foreground { get; }

	/// <summary>
	/// Gets the name of the style.
	/// </summary>
	/// <value>The name.</value>
	public string Name { get; }

	/// <summary>
	/// Gets the style matching the specified name.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>The ConsoleStyle.</returns>
	public static ConsoleStyle Get(StyleName name)
	{
		switch (name)
		{
			case StyleName.Default:
				return Default;
			case StyleName.Error:
				return Error;
			case StyleName.Info:
				return Info;
			case StyleName.Ok:
				return Ok;
			case StyleName.Warning:
				return Warning;
			default:
				return Default;
		}
	}
}