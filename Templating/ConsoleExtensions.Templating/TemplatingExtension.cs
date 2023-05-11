// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplatingExtension.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating;

using System.Globalization;

using Proxy;

/// <summary>
///     Class TemplatingExtension.
/// </summary>
public static class TemplatingExtension
{
    /// <summary>
    ///     Writes the supplied object using the specified template.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <param name="template">The template as a string. The template is parsed using the default template parser.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The Console Proxy.</returns>
    public static IConsoleProxy WriteTemplate(
        this IConsoleProxy proxy,
        string template,
        object arg = null,
        CultureInfo culture = null)
    {
        var parsed = TemplateParser.Default.Parse(template);
        parsed.Render(proxy, arg, culture);
        return proxy;
    }

    /// <summary>
    ///     Writes the supplied object using the specified template.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <param name="template">The template.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The Console Proxy.</returns>
    public static IConsoleProxy WriteTemplate(
        this IConsoleProxy proxy,
        Template template,
        object arg = null,
        CultureInfo culture = null)
    {
        template.Render(proxy, arg, culture);

        return proxy;
    }
}