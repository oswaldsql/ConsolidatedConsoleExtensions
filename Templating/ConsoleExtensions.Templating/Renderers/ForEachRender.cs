// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForEachRender.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Collections;
using System.Globalization;
using System.Reflection;

using Proxy;

/// <summary>
///     Class ForEachRender. Iterates over a enumeration of objects and renders each using the nested template.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class ForEachRender : Renderer
{
    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        if (arg == null)
        {
            return;
        }

        object value;
        if (string.IsNullOrEmpty(this.Config))
        {
            value = arg;
        }
        else
        {
            var property = arg.GetType().GetRuntimeProperty(this.Config);
            value = property?.GetValue(arg);
        }

        if (!(value is IEnumerable enumerable))
        {
            foreach (var subRenderer in this.SubRenderes)
            {
                subRenderer.Render(proxy, value, culture);
            }
        }
        else
        {
            foreach (var o in enumerable)
            {
                foreach (var subRenderer in this.SubRenderes)
                {
                    subRenderer.Render(proxy, o, culture);
                }
            }
        }
    }
}