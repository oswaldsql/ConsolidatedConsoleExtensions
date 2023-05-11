// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawTextToken.cs" company="Lasse Sj�rup">
//   Copyright (c) 2019 Lasse Sj�rup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

/// <summary>
///     Class RawTextToken. Represents a token containing raw text to be rendered.
///     Implements the <see cref="ConsoleExtensions.Templating.Token.Token" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Templating.Token.Token" />
internal class RawTextToken : Token
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RawTextToken" /> class.
    /// </summary>
    /// <param name="substring">The substring.</param>
    public RawTextToken(string substring)
        : base(substring)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RawTextToken" /> class.
    /// </summary>
    /// <param name="first">The first token.</param>
    /// <param name="second">The second token.</param>
    public RawTextToken(Token first, Token second)
        : base(first?.Substring + second.Substring)
    {
    }

    /// <summary>
    ///     Gets the type of the token.
    /// </summary>
    public override TokenType Type => TokenType.Text;
}