// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelCommand.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser;

using System.Reflection;

/// <summary>
///     Class ModelCommand. Represents a model command.
/// </summary>
/// <param name="Name">Name of the command.</param>
/// <param name="Method">Method to be called when the command is issued.</param>
/// <param name="Source">Object on which the method should be called.</param>
/// <param name="DisplayName">Display name of the command.</param>
/// <param name="Description">Optional description of the command.</param>
public record ModelCommand(string Name, MethodInfo Method, object Source, string DisplayName, string Description);