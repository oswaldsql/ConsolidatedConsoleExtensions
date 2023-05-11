// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenizerExtensions.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating;

using System.Collections.Generic;

using Token;

/// <summary>
///     Class TokenizerExtensions. Extends the token with a optimization method.
/// </summary>
internal static class TokenizerExtensions
{
    /// <summary>
    ///     Optimizes the specified tokens.
    /// </summary>
    /// <param name="tokens">The tokens to be optimized.</param>
    /// <returns>A Enumerable of tokens.</returns>
    public static IEnumerable<Token.Token> Optimize(this IEnumerable<Token.Token> tokens)
    {
        RawTextToken prevToken = null;
        foreach (var current in tokens)
        {
            if (current is RawTextToken)
            {
                prevToken = new RawTextToken(prevToken, current);
            }
            else
            {
                if (prevToken != null)
                {
                    yield return prevToken;
                }

                prevToken = null;
                yield return current;
            }
        }

        if (prevToken != null)
        {
            yield return prevToken;
        }
    }
}