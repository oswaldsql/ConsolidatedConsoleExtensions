// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoolConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

/// <summary>
/// Converts a empty argument to true.
/// </summary>
/// <seealso cref="ConsoleExtensions.Commandline.Converters.IValueConverter" />
public class EmptyArgumentConverter : IValueConverter
{
    /// <inheritdoc />
    public ConverterPriority Priority => ConverterPriority.First;

    /// <inheritdoc />
    public bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        result = null;
        return false;
    }

    /// <inheritdoc />
    public bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result)
    {
        if (type == typeof(bool) && string.IsNullOrEmpty(source))
        {
            result = true;
            return true;
        }

        result = null;
        return false;
    }
}