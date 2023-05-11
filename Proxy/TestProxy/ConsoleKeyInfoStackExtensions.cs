namespace ConsoleExtensions.Proxy.TestHelpers;

using System;
using System.Collections.Generic;

/// <summary>
///     Extensions for the ConsoleKeyInfoStack class.
/// </summary>
public static class ConsoleKeyInfoStackExtensions
{
    /// <summary>
    ///     Adds the specified values as a set of keys to the stack of keys.
    /// </summary>
    /// <param name="target">The target queue.</param>
    /// <param name="values">The keys to add.</param>
    /// <returns>The key queue.</returns>
    public static Queue<ConsoleKeyInfo> Add(this Queue<ConsoleKeyInfo> target, string values)
    {
        var charArray = values.ToCharArray();
        foreach (var c in charArray)
        {
            target.Enqueue(new ConsoleKeyInfo(c, ConsoleKey.Separator, false, false, false));
        }

        return target;
    }

    /// <summary>
    ///     Adds the specified key to the stack of keys.
    /// </summary>
    /// <param name="target">The target queue.</param>
    /// <param name="value">The key to add.</param>
    /// <param name="controlKeys">The control keys.</param>
    /// <returns>The key queue.</returns>
    public static Queue<ConsoleKeyInfo> Add(this Queue<ConsoleKeyInfo> target, ConsoleKey value,
        ControlKeys controlKeys = ControlKeys.None)
    {
        target.Enqueue(new ConsoleKeyInfo(' ', value, controlKeys.HasFlag(ControlKeys.Shift),
            controlKeys.HasFlag(ControlKeys.Alt), controlKeys.HasFlag(ControlKeys.Control)));
        return target;
    }
}