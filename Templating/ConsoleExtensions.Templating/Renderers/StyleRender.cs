// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleRender.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Globalization;

using Proxy;

/// <summary>
///     Class StyleRender. Sets the front and back color of the console, renders the SubRenderes and reset to the previous
///     colors.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class StyleRender : Renderer
{
    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        if (this.Template.Styles.TryGetValue(this.Config, out var style))
        {
            proxy.GetStyle(out var original);
            proxy.Style(style);
            foreach (var subRenderer in this.SubRenderes)
            {
                subRenderer.Render(proxy, arg, culture);
            }

            proxy.Style(original);
        }
        else
        {
            foreach (var subRenderer in this.SubRenderes)
            {
                subRenderer.Render(proxy, arg, culture);
            }
        }
    }
}