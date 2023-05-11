// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandFactory.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

using System;
using System.Collections.Generic;
using System.Linq;

using Token;

/// <summary>
///     Class CommandFactory. Initializes the command that the template can use.
/// </summary>
internal class CommandFactory
{
    /// <summary>
    ///     The commands available.
    /// </summary>
    private readonly Dictionary<string, Func<Renderer>> commands =
        new Dictionary<string, Func<Renderer>>(StringComparer.OrdinalIgnoreCase)
        {
            { "HR", () => new HrRender() },
            { "BR", () => new LineBreakRender() },
            { "ClearLine", () => new ClearLineRender() },
            { "C", () => new ColorRender() },
            { "S", () => new StyleRender() },
            { "If", () => new IfRender() },
            { "IfNot", () => new IfNotRender() },
            { "With", () => new WithRender() },
            { "ForEach", () => new ForEachRender() }
        };

    /// <summary>
    ///     Creates a render tree based on the token.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="token">The token.</param>
    /// <returns>The created renderer.</returns>
    public Renderer Create(Template template, Token token)
    {
        var strings = token.Substring.Split(new[] { ':' }, 2);
        var command = strings.First();
        var isClosed = command.EndsWith("/");
        command = command.Trim(' ', '/');
        var config = strings.Length == 2 ? strings[1] : string.Empty;
        var renderer = this.commands[command]();

        renderer.Config = config;

        // renderer.SubRenderes = subRenderes.ToArray();
        renderer.Template = template;
        renderer.IsClosed = isClosed;
        return renderer;
    }
}