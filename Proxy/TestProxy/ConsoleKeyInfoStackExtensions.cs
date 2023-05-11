namespace ConsoleExtensions.Proxy.TestHelpers;

using System;
using System.Collections.Generic;

public static class ConsoleKeyInfoStackExtensions
{
	public static Queue<ConsoleKeyInfo> Add(this Queue<ConsoleKeyInfo> target, string values)
	{
		char[] charArray = values.ToCharArray();
		foreach (char c in charArray)
		{
			target.Enqueue(new ConsoleKeyInfo(c, ConsoleKey.Separator, false, false, false));
		}

		return target;
	}

	public static Queue<ConsoleKeyInfo> Add(this Queue<ConsoleKeyInfo> target, ConsoleKey value, ControlKeys controlKeys = ControlKeys.None)
	{
		target.Enqueue(new ConsoleKeyInfo(' ', value, controlKeys.HasFlag(ControlKeys.Shift), controlKeys.HasFlag(ControlKeys.Alt), controlKeys.HasFlag(ControlKeys.Control)));
		return target;
	}
}