// --------------------------------------------------------------------------------------------------------------------
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
public class EnumConverter : IValueConverter
{
    /// <inheritdoc />
    public ConverterPriority Priority => ConverterPriority.Default;

    /// <inheritdoc />
    public virtual bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        if (!source.GetType().IsEnum)
        {
            result = "";
            return false;
        }

        result = source.ToString();
        return true;
    }

    /// <inheritdoc />
    public bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result)
    {
        if (!type.IsEnum)
        {
            result = "";
            return false;
        }

        if (source == null)
        {
            result = Enum.ToObject(type, 0);
            return true;
        }

        try
        {
            result = Enum.Parse(type, source, true);
            return true;
        }
        catch (Exception)
        {
            result = "";
            return false;
        }
    }
}