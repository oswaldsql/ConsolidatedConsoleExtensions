namespace ConsoleExtensions.Proxy;

using System;

public static class StyleExtensions
{
    /// <inheritdoc />
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

    /// <inheritdoc />
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy Style(this IConsoleProxy proxy, ConsoleStyle style)
    {
        proxy.SetColor(style.Foreground, style.Background);
        return proxy;
    }

    /// <inheritdoc />
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy Style(this IConsoleProxy proxy, StyleName name)
    {
        proxy.Style(ConsoleStyle.Get(name));
        return proxy;
    }

	
    /// <inheritdoc />
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy ResetStyle(this IConsoleProxy proxy)
    {
        proxy.ResetColor();
        return proxy;
    }
	
    /// <inheritdoc />
    /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static IConsoleProxy GetStyle(this IConsoleProxy proxy, out ConsoleStyle style)
    {
        style = new ConsoleStyle("current", proxy.ForegroundColor, proxy.BackgroundColor);
        return proxy;
    }

    public static IConsoleProxy WriteLine(this IConsoleProxy proxy, string value, ConsoleStyle style) => 
        proxy.GetStyle(out var currentStyle).Style(style).WriteLine(value).Style(currentStyle);
}