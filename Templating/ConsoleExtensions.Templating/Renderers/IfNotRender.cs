// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfNotRender.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Renderers;

/// <summary>
///     Class IfNotRender. Renders the nested template if the argument in config is falsy.
///     Implements the <see cref="ConsoleExtensions.Templating.Renderers.IfRender" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Renderers.IfRender" />
internal class IfNotRender : IfRender
{
    /// <summary>
    ///     Determines whether the specified SubRenderes should be rendered.
    /// </summary>
    /// <param name="clause">The clause.</param>
    /// <returns><c>true</c> if the SubRenderes should be rendered; otherwise, <c>false</c>.</returns>
    internal override bool ShouldBeRendered(object clause)
    {
        return !base.ShouldBeRendered(clause);
    }
}