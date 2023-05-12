// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParsedArguments.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Arguments;

using System.Collections.Generic;

/// <summary>
///     Class ParsedArguments. The console arguments as mapped by the arguments parser.
/// </summary>
public record ParsedArguments(string Command, string[] Arguments, Dictionary<string, List<string>> Properties);