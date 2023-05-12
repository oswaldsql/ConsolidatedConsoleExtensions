namespace ConsoleExtensions.Proxy.TestHelpers;

using System;

/// <summary>
/// Exception thrown when the test proxy has no more keys in the key queue.
/// </summary>
/// <seealso cref="System.Exception" />
public class NoMoreKeysInKeyQueue : Exception
{
}