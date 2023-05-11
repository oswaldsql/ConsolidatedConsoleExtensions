namespace ConsoleExtensions.Proxy;

using System;

/// <summary>
///     Extensions for the IConsoleProxy interface.
/// </summary>
public static class StyleExtensions
{
    /// <summary>
    ///     Sets the colors of the console.
    /// </summary>
    /// <param name="proxy">The current Console Proxy.</param>
    /// <param name="foreground">The foreground color.</param>
    /// <param name="background">The background color.</param>
    /// <returns>The current Console Proxy.</returns>
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy SetColor(this IConsoleProxy proxy, ConsoleColor? foreground, ConsoleColor? background)
    {
        if (foreground.HasValue)
        {
            proxy.ForegroundColor = foreground.Value;
        }

        if (background.HasValue)
        {
            proxy.BackgroundColor = background.Value;
        }

        return proxy;
    }

    /// <summary>
    ///     Styles the output of the console based on a predefined style.
    /// </summary>
    /// <param name="proxy">The current Console Proxy.</param>
    /// <param name="style">The style.</param>
    /// <returns>The current Console Proxy.</returns>
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy Style(this IConsoleProxy proxy, ConsoleStyle style)
    {
        proxy.SetColor(style.Foreground, style.Background);
        return proxy;
    }


    /// <summary>
    ///     Styles the output of the console based on a predefined style.
    /// </summary>
    /// <param name="proxy">The current Console Proxy.</param>
    /// <param name="name">The name.</param>
    /// <returns>The current Console Proxy.</returns>
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy Style(this IConsoleProxy proxy, StyleName name)
    {
        proxy.Style(ConsoleStyle.Get(name));
        return proxy;
    }

    /// <summary>
    ///     Resets the color.
    /// </summary>
    /// <param name="proxy">The current Console Proxy.</param>
    /// <returns>The current Console Proxy.</returns>
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy ResetStyle(this IConsoleProxy proxy)
    {
        proxy.ResetColor();
        return proxy;
    }


    /// <summary>
    ///     Gets the current style of the console.
    /// </summary>
    /// <param name="proxy">The current Console Proxy.</param>
    /// <param name="style">The current style of the console.</param>
    /// <returns>The current Console Proxy.</returns>
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy GetStyle(this IConsoleProxy proxy, out ConsoleStyle style)
    {
        style = new ConsoleStyle("current", proxy.ForegroundColor, proxy.BackgroundColor);
        return proxy;
    }

    /// <summary>
    ///     Writes a line break to the console.
    /// </summary>
    /// <param name="proxy">The current Console Proxy.</param>
    /// <param name="value">The value.</param>
    /// <param name="style">The style to use for the text.</param>
    /// <returns>The current Console Proxy.</returns>
    public static IConsoleProxy WriteLine(this IConsoleProxy proxy, string value, ConsoleStyle style)
    {
        return proxy.GetStyle(out var currentStyle).Style(style).WriteLine(value).Style(currentStyle);
    }
}