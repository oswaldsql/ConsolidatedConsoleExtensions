// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

/// <summary>
/// Class TimeSpanValueConverter.
/// Implements the <see cref="ConsoleExtensions.Commandline.Converters.IValueConverter" />
/// </summary>
/// <seealso cref="ConsoleExtensions.Commandline.Converters.IValueConverter" />
public class TimeSpanValueConverter : ValueConverterBack
{
    /// <summary>
    /// Gets the priority of the converter.
    /// </summary>
    /// <value>The priority.</value>
    public override  ConverterPriority Priority => ConverterPriority.Default;

    /// <summary>
    /// Determines whether this instance can convert the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
    protected override  bool CanConvert(Type type)
    {
        return type == typeof(TimeSpan);
    }

    /// <summary>
    /// Converts a value to a string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    protected  override string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider)
    {
        return source.ToString();
    }

    /// <summary>
    /// Converts a string to a value of the specified type.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="type">The type.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <returns>A object of the specified type.</returns>
    protected override object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
    {
        return TimeSpan.Parse(source);
    }
}