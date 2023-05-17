﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

/// <summary>
///     Class EnumConverter. Converts enums to and from string.
///     Implements the <see cref="IValueConverter" />
/// </summary>
/// <seealso cref="IValueConverter" />
public class EnumConverter : ValueConverterBack
{
    /// <summary>
    ///     Gets the priority of the converter.
    /// </summary>
    public override  ConverterPriority Priority => ConverterPriority.Default;

    /// <summary>
    ///     Determines whether this instance can convert the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
    protected override  bool CanConvert(Type type)
    {
        return type.IsEnum;
    }

    /// <summary>
    ///     Converts a value to a string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A <see cref="T:System.String" /> that represents this instance.</returns>
    protected override  string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider)
    {
        return source.ToString();
    }

    /// <summary>
    ///     Converts a string to a value of the specified type.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="type">The type.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A object of the specified type.</returns>
    protected override  object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
    {
        return Enum.Parse(type, source, true);
    }
}