// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenType.cs" company="Lasse Sj�rup">
//   Copyright (c) 2019 Lasse Sj�rup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

/// <summary>
///     Enum TokenType. Defines the types of token that can be parsed.
/// </summary>
internal enum TokenType
{
    /// <summary>
    ///     The substitution token used as placeholder for values.
    /// </summary>
    Substitution = 1,

    /// <summary>
    ///     The text token used for raw text.
    /// </summary>
    Text = 2,

    /// <summary>
    ///     The command token used for the different commands.
    /// </summary>
    Command = 3,

    /// <summary>
    ///     The end command used as a placeholder for ending iterations.
    /// </summary>
    EndCommand = 4
}