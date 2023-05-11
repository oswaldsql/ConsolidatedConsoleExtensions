// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfRender.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System;
using System.Collections;
using System.Globalization;

using Proxy;

/// <summary>
///     Class IfRender. Renders the nested template if the argument in config is truthy. A value is truthy unless it is
///     false, 0, a empty string or array.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class IfRender : Renderer
{
    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        var o = this.GetValueFromPropertyString(arg, this.Config);

        var shouldBeRendered = this.ShouldBeRendered(o);
        if (shouldBeRendered)
        {
            foreach (var subRenderer in this.SubRenderes)
            {
                subRenderer.Render(proxy, arg, culture);
            }
        }
    }

    /// <summary>
    ///     Determines whether the specified SubRenderes should be rendered.
    /// </summary>
    /// <param name="clause">The clause.</param>
    /// <returns><c>true</c> if the SubRenderes should be rendered; otherwise, <c>false</c>.</returns>
    internal virtual bool ShouldBeRendered(object clause)
    {
        switch (clause)
        {
            case null:
                return false;
            case bool b:
                return b;
            case string s:
                return !string.IsNullOrEmpty(s);
            case IEnumerable enumerable:
                return enumerable.GetEnumerator().MoveNext();
            case IConvertible convertible:
            {
                var d = convertible.ToDouble(CultureInfo.InvariantCulture);
                return Math.Abs(d) > 0;
            }
        }

        return true;
    }
}