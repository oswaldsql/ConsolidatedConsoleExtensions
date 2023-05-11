namespace ConsoleExtensions.Proxy;

using System;
using System.Text;

/// <summary>
/// Class ConsoleProxy.
/// </summary>
/// <seealso cref="ConsoleExtensions.Proxy.IConsoleProxy" />
public class ConsoleProxy : IConsoleProxy
{
	/// <summary>
	/// Initializes static members of the <see cref="ConsoleProxy"/> class.
	/// </summary>
	static ConsoleProxy()
	{
		LocalInstance = new ConsoleProxy();
	}

	/// <summary>
	/// Prevents a default instance of the <see cref="ConsoleProxy"/> class from being created from the outside.
	/// </summary>
	private ConsoleProxy()
	{
	}

	/// <inheritdoc />
	public int WindowWidth => Console.WindowWidth;

	public ConsoleColor ForegroundColor
	{
		get => Console.ForegroundColor;
		set => Console.ForegroundColor = value;
	}

	public ConsoleColor BackgroundColor
	{
		get => Console.BackgroundColor;
		set => Console.BackgroundColor = value;
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	private static IConsoleProxy LocalInstance { get; }

	/// <summary>
	/// Gets the shared instance of the console proxy.
	/// </summary>
	/// <returns>
	/// Instance of the console proxy wrapping the static System.Console.
	/// </returns>
	public static IConsoleProxy Instance()
	{
		return LocalInstance;
	}

	/// <inheritdoc />
	public IConsoleProxy Beep()
	{
		Console.Beep();
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy Clear()
	{
		Console.Clear();
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy GetPosition(out CursorPoint point)
	{
		point = new CursorPoint(Console.CursorTop, Console.CursorLeft);
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy GetTitle(out string title)
	{
		title = Console.Title;
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy HideCursor()
	{
		Console.CursorVisible = false;
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy ReadKey(out ConsoleKeyInfo key, bool intercept = false)
	{
		key = Console.ReadKey(intercept);
		return this;
	}

	/// <inheritdoc />
	/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
	/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
	public IConsoleProxy ReadLine(out string result)
	{
		var sb = new StringBuilder();
		while (true)
		{
			int ch = Console.Read();
			if (ch == -1) break;
			if (ch == '\r' || ch == '\n')
			{
				if (ch == '\r' && Console.In.Peek() == '\n')
				{
					Console.Read();
				}

				result = sb.ToString();
				return this;
			}
			sb.Append((char)ch);
		}
		if (sb.Length > 0)
		{
			result = sb.ToString();
			return this;
		}

		result = null;
		
		result = Console.ReadLine();
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy SetPosition(CursorPoint point)
	{
		Console.CursorTop = point.Top;
		Console.CursorLeft = point.Left;
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy ShowCursor()
	{
		Console.CursorVisible = true;
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy SetTitle(string title)
	{
		Console.Title = title;
		return this;
	}

	/// <inheritdoc />
	/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
	public IConsoleProxy Write(string value)
	{
		Console.Write(value);
		return this;
	}

	/// <inheritdoc />
	/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
	public IConsoleProxy WriteLine(string value = "")
	{
		Console.WriteLine(value);
		return this;
	}

	/// <inheritdoc />
	public IConsoleProxy ResetColor()
	{
		Console.ResetColor();
		return this;
	}

	/// <inheritdoc />
	public event ConsoleCancelEventHandler? CancelKeyPress
	{
		add => Console.CancelKeyPress += value;
		remove => Console.CancelKeyPress -= value;
	}

	/// <inheritdoc />
	public bool TreatControlCAsInput
	{
		get => Console.TreatControlCAsInput;
		set => Console.TreatControlCAsInput = value;
	}
}