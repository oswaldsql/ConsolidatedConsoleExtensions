// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Token.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

/// <summary>
///     Class Token. Serves as a base class for tokens.
/// </summary>
internal abstract class Token
{
    /// <summary>
    ///     The substring representing the token.
    /// </summary>
    internal readonly string Substring;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Token" /> class.
    /// </summary>
    /// <param name="substring">The substring.</param>
    public Token(string substring)
    {
        this.Substring = substring;
    }

    /// <summary>
    ///     Gets the type.
    /// </summary>
    public abstract TokenType Type { get; }

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return this.Substring;
    }
}