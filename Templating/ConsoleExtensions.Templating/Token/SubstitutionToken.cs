// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutionToken.cs" company="Lasse Sj�rup">
//   Copyright (c) 2019 Lasse Sj�rup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

/// <summary>
///     Class SubstitutionToken. Represents a token that serves as a placeholder for values.
///     Implements the <see cref="ConsoleExtensions.Templating.Token.Token" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Token.Token" />
internal class SubstitutionToken : Token
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SubstitutionToken" /> class.
    /// </summary>
    /// <param name="substring">The substring.</param>
    public SubstitutionToken(string substring)
        : base(substring)
    {
    }

    /// <summary>
    ///     Gets the type of the token.
    /// </summary>
    public override TokenType Type => TokenType.Substitution;

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return "sub:" + base.ToString();
    }
}