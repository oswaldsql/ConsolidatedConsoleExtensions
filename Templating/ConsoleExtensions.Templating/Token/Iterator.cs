// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Iterator.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Token;

using System.Linq;

/// <summary>
///     Class Iterator. Works as a extended char array.
/// </summary>
internal class Iterator
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Iterator" /> class.
    /// </summary>
    /// <param name="source">The source.</param>
    public Iterator(string source)
    {
        this.Chars = source.ToCharArray();
        this.Length = this.Chars.Length;
    }

    /// <summary>
    ///     Gets the chars representing the original string.
    /// </summary>
    public char[] Chars { get; }

    /// <summary>
    ///     Gets the char at the current index.
    /// </summary>
    public char Current => this.Chars[this.Index];

    /// <summary>
    ///     Gets a value indicating whether this <see cref="Iterator" /> is end of line.
    /// </summary>
    public bool Eol => this.Index >= this.Length;

    /// <summary>
    ///     Gets the index of the pointer.
    /// </summary>
    public int Index { get; private set; }

    /// <summary>
    ///     Gets the length of the string.
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     Gets the next char and moves the pointer.
    /// </summary>
    public char Next => this.Index == this.Length - 1 ? '\0' : this.Chars[this.Index + 1];

    /// <summary>
    ///     Gets the start position of the current substring.
    /// </summary>
    public int Start { get; private set; }

    /// <summary>
    ///     Gets the string formed by the chars from start to the current index including the stop chars.
    /// </summary>
    /// <returns>The sub string formed by the delimiters.</returns>
    public string GetExternal()
    {
        var result = new string(this.Chars, this.Start, this.Index - this.Start);
        return result;
    }

    /// <summary>
    ///     Gets the string formed by the chars from start to the current index excluding the stop chars.
    /// </summary>
    /// <returns>The sub string formed by the delimiters.</returns>
    public string GetInternal()
    {
        var result = new string(this.Chars, this.Start + 1, this.Index - this.Start - 1);
        this.Index++;
        return result;
    }

    /// <summary>
    ///     Moved the current index the specified count.
    /// </summary>
    /// <param name="count">The count.</param>
    public void Iterate(int count = 1)
    {
        this.Index += count;
    }

    /// <summary>
    ///     moved the index until one of the stop chars are found.
    /// </summary>
    /// <param name="stopChars">The stop chars.</param>
    public void IterateUntil(params char[] stopChars)
    {
        while (!this.Eol && !stopChars.Contains(this.Chars[this.Index]))
        {
            this.Index++;
        }
    }

    /// <summary>
    ///     Resets the start to the specified index.
    /// </summary>
    public void ResetStart()
    {
        this.Start = this.Index;
    }
}