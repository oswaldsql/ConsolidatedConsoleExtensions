// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootRenderer.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Globalization;

using Proxy;

/// <summary>
///     Class RootRenderer. Serves as the root of the template.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class RootRenderer : Renderer
{
    /// <summary>
    ///     The renderers
    /// </summary>
    private readonly Renderer[] renderers;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RootRenderer" /> class.
    /// </summary>
    /// <param name="renderers">The renderers.</param>
    public RootRenderer(Renderer[] renderers)
    {
        this.renderers = renderers;
    }

    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        foreach (var renderer in this.renderers)
        {
            renderer.Render(proxy, arg, culture);
        }
    }
}