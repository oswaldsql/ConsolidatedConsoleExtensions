// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

/// <summary>
///     Interface IValueConverter. Provides a way to convert strings to objects and back again when the converts is unable
///     to.
/// </summary>
public interface IValueConverter
{
    /// <summary>
    ///     Gets the priority of the converter.
    /// </summary>
    ConverterPriority Priority { get; }

    /// <summary>
    ///     Determines whether this instance can convert the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
    bool CanConvert(Type type);

    /// <summary>
    ///     Converts a value to a string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider);

    /// <summary>
    ///     Converts a string to a value of the specified type.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="type">The type.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A object of the specified type.</returns>
    object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider);
}