// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextRenderer.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Globalization;

using Proxy;

/// <summary>
///     Class TextRenderer. Renders the raw content of the template.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class TextRenderer : Renderer
{
    /// <summary>
    ///     The value
    /// </summary>
    private readonly string value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextRenderer" /> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public TextRenderer(string value)
    {
        this.value = value;
    }

    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        proxy.Write(this.value);
    }
}