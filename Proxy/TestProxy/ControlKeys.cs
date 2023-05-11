namespace ConsoleExtensions.Proxy.TestHelpers;

using System;

/// <summary>
///     Control keys.
/// </summary>
[Flags]
public enum ControlKeys
{
    /// <summary>
    ///     No control keys.
    /// </summary>
    None = 0,


    /// <summary>
    ///     The Shift key.
    /// </summary>
    Shift = 1,

    /// <summary>
    ///     The alt key.
    /// </summary>
    Alt = 2,

    /// <summary>
    ///     The control key.
    /// </summary>
    Control = 4,
}