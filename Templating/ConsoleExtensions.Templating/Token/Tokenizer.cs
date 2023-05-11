// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tokenizer.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

using System;
using System.Collections.Generic;

/// <summary>
///     Class Tokenizer. Converts a string to a enumeration of tokens.
/// </summary>
internal class Tokenizer
{
    /// <summary>
    ///     The tokenizers.
    /// </summary>
    private static readonly Dictionary<char, Func<Iterator, Token>> Tokenizers;

    /// <summary>
    ///     Initializes static members of the <see cref="Tokenizer" /> class.
    /// </summary>
    static Tokenizer()
    {
        Tokenizers = new Dictionary<char, Func<Iterator, Token>>
        {
            { '[', TokenizeCommand }, { '{', TokenizeSubstitution }
        };
    }

    /// <summary>
    ///     Tokenizes the specified template string.
    /// </summary>
    /// <param name="templateString">The templateString.</param>
    /// <returns>A Enumerable of Tokens.</returns>
    /// <exception cref="InvalidTemplateException">Thrown if the tokens dos not match up.</exception>
    public IEnumerable<Token> Tokenize(string templateString)
    {
        var iterator = new Iterator(templateString);

        while (!iterator.Eol)
        {
            var previousStart = iterator.Index;
            iterator.ResetStart();

            if (Tokenizers.TryGetValue(iterator.Current, out var parser))
            {
                yield return parser(iterator);
            }
            else
            {
                yield return RawToken(iterator);
            }

            if (previousStart == iterator.Index)
            {
                throw new InvalidTemplateException();
            }
        }
    }

    /// <summary>
    ///     Iterates until the next stop character.
    /// </summary>
    /// <param name="iterator">The iterator.</param>
    /// <returns>A Token containing the text until next stop char.</returns>
    private static Token RawToken(Iterator iterator)
    {
        iterator.IterateUntil('[', '{');

        return new RawTextToken(iterator.GetExternal());
    }

    /// <summary>
    ///     Creates a command token from the next part of the string.
    /// </summary>
    /// <param name="iterator">The iterator.</param>
    /// <returns>A command token.</returns>
    private static Token TokenizeCommand(Iterator iterator)
    {
        if (iterator.Next == '[')
        {
            iterator.Iterate(2);
            return new RawTextToken("[");
        }

        iterator.IterateUntil(']');

        var c = iterator.GetInternal();
        return c.StartsWith("/") ? new EndCommandToken(c.Substring(1)) : new CommandToken(c);
    }

    /// <summary>
    ///     Creates a substitution token from the next part of the string.
    /// </summary>
    /// <param name="iterator">The iterator.</param>
    /// <returns>A substitution token.</returns>
    private static Token TokenizeSubstitution(Iterator iterator)
    {
        if (iterator.Next == '{')
        {
            iterator.Iterate(2);
            return new RawTextToken("{");
        }

        iterator.IterateUntil('}');

        return new SubstitutionToken(iterator.GetInternal());
    }
}