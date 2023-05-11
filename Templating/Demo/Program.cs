// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo;

using System;
using System.Globalization;
using System.Reflection;

using ConsoleExtensions.Proxy;
using ConsoleExtensions.Templating;

/// <summary>
///     Class Program.
/// </summary>
internal class Program
{
    /// <summary>
    ///     Defines the entry point of the application.
    /// </summary>
    private static void Main()
    {
        var proxy = ConsoleProxy.Instance();

        var templateParser = TemplateParser.Default;
        templateParser.AddTypeTemplate<Version>(
            "[c:white]{Major}[/].[c:white]{Minor}[/][if:Build].[c:white]{Build}[if:Revision].[c:white]{Revision}[/][/][/][/]");
        templateParser.AddTypeTemplate<AssemblyCopyrightAttribute>("Copyright : {Copyright}");
        var assemblyInfoTemplate = templateParser.Parse(
            "[foreach:attribs] {} [/] [hr/][with:version]Name : {Name} [br/] Version :{Version}[/] [br/]");

        var assembly = typeof(Program).Assembly;

        var version = assembly.GetName();
        proxy.WriteTemplate(assemblyInfoTemplate, new { version, attribs = assembly.GetCustomAttributes(true) });

        proxy.WriteTemplate("[hr/][c:white] Hello world ![/][br/] How are you?[hr]");

        proxy.WriteTemplate(
            "Today is a {:dddd} in {:MMMM}[br/]",
            DateTimeOffset.UtcNow,
            CultureInfo.InvariantCulture);

        proxy.WriteTemplate(
            "and you have to [if:IsWorkDay]work[/if][ifnot:IsWorkDay]relax[/if][br/]",
            new { IsWorkDay = DateTimeOffset.UtcNow.DayOfWeek != DayOfWeek.Sunday });

        var consoleColors = Enum.GetNames(typeof(ConsoleColor));
        proxy.WriteTemplate("[hr/]There are {Length} console colors [foreach][br/] - {}[/][br/]", consoleColors);

        proxy.WriteTemplate("[hr/]The 5 build in styles are:[br/]").WriteTemplate("- [s:Default]Default[/][br/]")
            .WriteTemplate("- [s:Ok]OK[/][br/]").WriteTemplate("- [s:Info]Info[/][br/]")
            .WriteTemplate("- [s:Warning]Warning[/][br/]").WriteTemplate("- [s:Error]Error[/][br/]");
    }
}