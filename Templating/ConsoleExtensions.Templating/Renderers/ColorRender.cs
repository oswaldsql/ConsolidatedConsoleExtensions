// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorRender.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System;
using System.Globalization;

using Proxy;

/// <summary>
///     Class ColorRender. Sets the console to render in a specific color.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class ColorRender : Renderer
{
    /// <summary>
    ///     Instructs the renderer change color to the specified value, render all SubRenderes and the change the color back to the original color.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        var consoleColor = this.GetColorFromConfigValue();

        if (consoleColor == null)
        {
            foreach (var subRenderer in this.SubRenderes)
            {
                subRenderer.Render(proxy, arg, culture);
            }

            return;
        }

        proxy.GetStyle(out var original);
        proxy.Style(new ConsoleStyle("temp", consoleColor));
        foreach (var subRenderer in this.SubRenderes)
        {
            subRenderer.Render(proxy, arg, culture);
        }

        proxy.Style(original);
    }

    /// <summary>
    ///     Gets the color from configuration value.
    /// </summary>
    /// <returns>A color derived from the configuration.</returns>
    private ConsoleColor? GetColorFromConfigValue()
    {
        if (Enum.TryParse(this.Config, true, out ConsoleColor result))
        {
            return result;
        }

        return null;
    }
}