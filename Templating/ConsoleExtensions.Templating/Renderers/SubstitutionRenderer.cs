// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutionRenderer.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System;
using System.Globalization;
using System.Reflection;

using Proxy;

/// <summary>
///     Class SubstitutionRenderer.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.Renderer" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.Renderer" />
internal class SubstitutionRenderer : Renderer
{
    /// <summary>
    ///     The format separator
    /// </summary>
    private static readonly char[] FormatSeparator = { ':' };

    /// <summary>
    ///     The format
    /// </summary>
    private string format;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SubstitutionRenderer" /> class.
    /// </summary>
    /// <param name="value">The arg.</param>
    /// <param name="template">The template.</param>
    public SubstitutionRenderer(string value, Template template)
    {
        this.Config = value;
        
        var strings = value.Split(FormatSeparator, 2);
        this.PropertyName = strings[0];
        if (strings.Length == 2)
        {
            this.format = strings[1];
        }

        this.Template = template;
    }

    /// <summary>
    ///     Gets or sets the name of the property.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    ///     Instructs the renderer to render to the supplied proxy.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    public override void Render(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        var value = this.GetValueFromPropertyString(arg, this.PropertyName);
        if (value == null)
        {
            return;
        }

        if (this.format != null && value is IFormattable formatValue)
        {
            proxy.Write(formatValue.ToString(this.format, culture));
            return;
        }

        if (this.RenderUsingTypeTemplates(proxy, value, culture))
        {
            return;
        }

        proxy.Write(string.Format(culture, "{0}", value));
    }

    /// <summary>
    ///     If a specific template has be registered to a type the template is rendered.
    /// </summary>
    /// <param name="proxy">The proxy to render to.</param>
    /// <param name="arg">The object to render.</param>
    /// <param name="culture">The culture to use for the render.</param>
    /// <returns><c>true</c> if a matching template was found, <c>false</c> otherwise.</returns>
    private bool RenderUsingTypeTemplates(IConsoleProxy proxy, object arg, CultureInfo culture)
    {
        if (this.format != null)
        {
            return false;
        }

        var type = arg.GetType();
        while (type != null)
        {
            if (this.Template.TypeTemplates.TryGetValue(type, out var template))
            {
                if (this.Template.TypeConverters.TryGetValue(type, out var converter))
                {
                    arg = converter(arg);
                }

                proxy.WriteTemplate(template, arg, culture);
                return true;
            }

            type = type.GetTypeInfo().BaseType;
        }

        return false;
    }
}