// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Template.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating;

using System;
using System.Collections.Generic;
using System.Globalization;

using Proxy;
using Renderers;

/// <summary>
///     Class Template. Represents a template for rendering a object to a console proxy.
/// </summary>
public class Template
{
    /// <summary>
    ///     The parser used for parsing the template.
    /// </summary>
    private readonly TemplateParser parser;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Template" /> class.
    /// </summary>
    /// <param name="parser">The parser used for parsing the templates.</param>
    internal Template(TemplateParser parser)
    {
        this.parser = parser;
    }

    /// <summary>
    ///     Gets or sets the render tree.
    /// </summary>
    public Renderer RenderTree { get; set; }

    /// <summary>
    ///     Gets the styles available in the parser.
    /// </summary>
    public Dictionary<string, ConsoleStyle> Styles => this.parser.Style;

    /// <summary>
    ///     Gets the type converters in the parser.
    /// </summary>
    internal Dictionary<Type, Func<object, object>> TypeConverters => this.parser.TypeConverters;

    /// <summary>
    ///     Gets the type templates in the parser.
    /// </summary>
    internal Dictionary<Type, Template> TypeTemplates => this.parser.TypeTemplates;

    /// <summary>
    ///     Renders the specified proxy.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <param name="arg">The argument.</param>
    /// <param name="culture">The culture.</param>
    public void Render(IConsoleProxy proxy, object arg, CultureInfo culture = null)
    {
        this.RenderTree.Render(proxy, arg, culture);
    }
}