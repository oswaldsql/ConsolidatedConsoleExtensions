// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WithRender.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Globalization;
using System.Reflection;

using Proxy;

/// <summary>
///     Class WithRender. Replaces the object to be renderes for the SubRenderes.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class WithRender : Renderer
{
    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        object value;
        if (string.IsNullOrEmpty(this.Config))
        {
            value = arg;
        }
        else
        {
            var property = arg?.GetType().GetRuntimeProperty(this.Config);
            value = property?.GetValue(arg);
        }

        if (value != null)
        {
            foreach (var subRenderer in this.SubRenderes)
            {
                subRenderer.Render(proxy, value, culture);
            }
        }
    }
}