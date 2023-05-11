// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Renderer.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System.Globalization;
using System.Reflection;

using Proxy;

/// <summary>
///     Class Renderer. Serves as a base class for all renderes.
/// </summary>
public abstract class Renderer
{
    /// <summary>
    ///     Gets or sets a value indicating whether the renderer is self closing.
    /// </summary>
    public bool IsClosed { get; set; }

    /// <summary>
    ///     Gets or sets the configuration of the renderer.
    /// </summary>
    internal string Config { get; set; }

    /// <summary>
    ///     Gets or sets the sub renderes.
    /// </summary>
    internal Renderer[] SubRenderes { get; set; }

    /// <summary>
    ///     Gets or sets the template the renderer is based on.
    /// </summary>
    internal Template Template { get; set; }

    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public abstract void Render(IConsoleProxy proxy, object arg, CultureInfo culture);

    /// <summary>
    ///     Gets the value from from a object based on the property name.
    /// </summary>
    /// <param name="arg">The object to get the value from.</param>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The value of the property.</returns>
    internal object GetValueFromPropertyString(object arg, string propertyName)
    {
        if (propertyName == string.Empty)
        {
            return arg;
        }

        var properties = propertyName.Split('.');
        foreach (var name in properties)
        {
            // TODO : fix property casing issue
            var property = arg?.GetType().GetRuntimeProperty(name);
            if (property == null)
            {
                return null;
            }

            arg = property.GetValue(arg);
            if (arg == null)
            {
                return null;
            }
        }

        return arg;
    }
}