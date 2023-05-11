// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EndRenderer.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Globalization;

using Proxy;

/// <summary>
///     Class EndRenderer. Used as a placeholder of terminating a list of sub renderes.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class EndRenderer : Renderer
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EndRenderer" /> class.
    /// </summary>
    /// <param name="substring">The substring.</param>
    public EndRenderer(string substring)
    {
        this.Substring = substring;
    }

    /// <summary>
    ///     Gets the substring.
    /// </summary>
    public string Substring { get; }

    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
    }
}