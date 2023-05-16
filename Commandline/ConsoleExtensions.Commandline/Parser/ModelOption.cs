// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelOption.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser;

using System;
using System.Reflection;

/// <summary>
///     Model option container.
/// </summary>
/// <param name="Name">Name of option.</param>
/// <param name="Property">Property to use when setting the option.</param>
/// <param name="Source">Object on which the property should be set.</param>
/// <param name="DisplayName">Display name of the option.</param>
/// <param name="Description">Optional description of the option.</param>
public record ModelOption(string Name, PropertyInfo Property, object Source, string DisplayName, string Description);

/// <summary>
///     Extension methods for options.
/// </summary>
public static class ModelOptionExtensions
{
    /// <summary>
    ///     Returns the currents value of the option.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <returns>
    ///     A string representing the value.
    /// </returns>
    public static object CurrentValue(this ModelOption option)
    {
        return option.Property.GetMethod.Invoke(option.Source, Array.Empty<object>());
    }

    /// <summary>
    ///     Sets the specified value.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    public static void Set(this ModelOption option, object value)
    {
        option.Property.SetMethod.Invoke(option.Source, new[] {value});
    }
}