﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

/// <summary>
///     Class CustomValueConverter. Converts to and from a string to a objects.
///     Implements the <see cref="IValueConverter" />
/// </summary>
/// <typeparam name="T">The type of object to converts to and from.</typeparam>
/// <seealso cref="IValueConverter" />
public class CustomValueConverter<T> : ValueConverterBack
{
    /// <summary>
    ///     The function used when converting to string.
    /// </summary>
    private readonly Func<object, string> toString;

    /// <summary>
    ///     The function used when converting to object.
    /// </summary>
    private readonly Func<string, object> toValue;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CustomValueConverter{T}" /> class.
    /// </summary>
    /// <param name="toValue">The function used when converting to object.</param>
    /// <param name="toString">The function used when converting to string.</param>
    public CustomValueConverter(Func<string, T> toValue, Func<T, string> toString)
    {
        this.toString = o => toString((T)o);
        this.toValue = s => toValue(s);
    }

    /// <summary>
    ///     Gets the priority of the converter.
    /// </summary>
    public ConverterPriority Priority => ConverterPriority.First;

    /// <summary>
    ///     Determines whether this instance can convert the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
    protected override  bool CanConvert(Type type)
    {
        return type == typeof(T);
    }

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents the source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    protected override  string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider)
    {
        return this.toString(source);
    }

    /// <summary>
    ///     Converts a string to a value of the specified type.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="type">The type.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A object of the specified type.</returns>
    protected  override object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
    {
        return this.toValue(source);
    }
}