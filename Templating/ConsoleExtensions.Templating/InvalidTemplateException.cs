// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidTemplateException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating;

using System;

/// <summary>
///     Class InvalidTemplateException. Thrown when a template is invalid and can not be parsed.
///     Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="System.Exception" />
public class InvalidTemplateException : Exception
{
}