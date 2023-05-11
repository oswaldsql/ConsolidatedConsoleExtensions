// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandToken.cs" company="Lasse Sj�rup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

/// <summary>
///     Class CommandToken. Represents a token that is translated to a command.
///     Implements the <see cref="ConsoleExtensions.Templating.Token.Token" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Token.Token" />
internal class CommandToken : Token
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandToken" /> class.
    /// </summary>
    /// <param name="substring">The substring.</param>
    public CommandToken(string substring)
        : base(substring)
    {
    }

    /// <summary>
    ///     Gets the type of command.
    /// </summary>
    public override TokenType Type => TokenType.Command;

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents this instance as a string.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return "com:" + base.ToString();
    }
}