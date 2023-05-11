namespace ConsoleExtensions.Proxy;

using System;

/// <summary>
/// Interface IConsoleProxy. Abstracts the interaction with the console.
/// </summary>
public interface IConsoleProxy
{
	/// <summary>
	/// Gets the width of the console window.
	/// </summary>
	/// <value>The width of the window.</value>
	int WindowWidth { get; }

	/// <summary>
	/// Gets or sets the foreground color of the console
	/// </summary>
	/// <returns>A ConsoleColor that specifies the foreground color of the console; that is, the color of each character that is displayed. The default is gray.</returns>
	ConsoleColor ForegroundColor { get; set; }
	
	/// <summary>
	/// Gets or sets the background color of the console.
	/// </summary>
	/// <returns>A value that specifies the background color of the console; that is, the color that appears behind each character. The default is black.</returns>
	ConsoleColor BackgroundColor { get; set; }

	/// <summary>
	/// Plays the sound of a beep through the console speaker.
	/// </summary>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy Beep();

	/// <summary>
	/// Clears the console window.
	/// </summary>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy Clear();

	/// <summary>
	/// Gets the current position of the cursor.
	/// </summary>
	/// <param name="point">The current position of the cursor.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy GetPosition(out CursorPoint point);

	/// <summary>
	/// Gets the title to display in the console title bar.
	/// </summary>
	/// <param name="title">The string to be displayed in the title bar of the console.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy GetTitle(out string title);

	/// <summary>
	/// Hides the cursor.
	/// </summary>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy HideCursor();

	/// <summary>
	/// Obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.
	/// </summary>
	/// <param name="key">An object that describes the <see cref="T:System.ConsoleKey" /> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo" /> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers" /> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</param>
	/// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy ReadKey(out ConsoleKeyInfo key, bool intercept = false);

	/// <summary>
	/// Reads the next line of characters from the standard input stream.
	/// </summary>
	/// <param name="result">The next line of characters from the input stream.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy ReadLine(out string result);

	/// <summary>
	/// Sets the position of the cursor.
	/// </summary>
	/// <param name="point">The point.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy SetPosition(CursorPoint point);

	/// <summary>
	/// Sets the title to display in the console title bar.
	/// </summary>
	/// <param name="title">The string to be displayed in the title bar of the console.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy SetTitle(string title);

	/// <summary>
	/// Shows the cursor.
	/// </summary>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy ShowCursor();

	/// <summary>
	/// Writes the specified value to the console.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy Write(string value);

	/// <summary>
	/// Writes a line break to the console.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>The current Console Proxy.</returns>
	IConsoleProxy WriteLine(string value = "");

	/// <summary>
	/// Sets the foreground and background console colors to their defaults
	/// </summary>
	/// <returns>The current console proxy.</returns>
	IConsoleProxy ResetColor();

	/// <summary>
	/// Occurs when the Control modifier key (Ctrl) and either the C console key (C) or the Break key are pressed simultaneously (Ctrl+C or Ctrl+Break).
	/// </summary>
	event ConsoleCancelEventHandler? CancelKeyPress;

	/// <summary>
	/// Gets or sets a value indicating whether the combination of the Control modifier key and C console key (Ctrl+C) is treated as ordinary input or as an interruption that is handled by the operating system.
	/// </summary>
	bool TreatControlCAsInput { get; set; }
}