﻿// --------------------------------------------------------------------------------------------------------------------
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
    /// Converts a value to a string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result);

    /// <summary>
    /// Tries to convert a string source to a type safe value.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="type">The type.</param>
    /// <param name="customAttributeProvider">The custom attribute provider.</param>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result);
}
