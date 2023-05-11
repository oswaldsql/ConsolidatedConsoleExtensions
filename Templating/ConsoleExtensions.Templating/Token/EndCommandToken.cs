// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EndCommandToken.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ConsoleExtensions.Templating.Token;

/// <summary>
/// Class EndCommandToken. Represents the end of a command.
/// Implements the <see cref="ConsoleExtensions.Templating.Token.Token" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Token.Token" />
internal class EndCommandToken : Token
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EndCommandToken"/> class.
    /// </summary>
    /// <param name="substring">The substring.</param>
    public EndCommandToken(string substring)
        : base(substring)
    {
    }

    /// <summary>
    /// Gets the type of the token.
    /// </summary>
    public override TokenType Type => TokenType.EndCommand;

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return "endCom:" + base.ToString();
    }
}