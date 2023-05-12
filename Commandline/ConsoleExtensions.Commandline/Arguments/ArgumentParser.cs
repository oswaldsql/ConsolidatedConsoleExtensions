// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentParser.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Arguments;

using System;
using System.Collections.Generic;

/// <summary>
///     Class ArgumentParser. Extracts information about the arguments and maps them as a command, arguments and options.
/// </summary>
public static class ArgumentParser
{
    /// <summary>
    ///     Parses the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The resulting ParsedArguments object.</returns>
    public static ParsedArguments Parse(params string[] args)
    {
        var command = "";
        var arguments = new List<string>();
        var properties = new Dictionary<string, List<string>>();

        if (args.Length == 0)
        {
            return new ParsedArguments("Help", Array.Empty<string>(), new Dictionary<string, List<string>>());;
        }

        var queue = new Queue<string>(args);

        if (!IsProp(queue.Peek()))
        {
            command = queue.Dequeue();
        }

        var valueList = arguments;
        while (queue.Count > 0)
        {
            var next = queue.Dequeue();
            if (IsProp(next))
            {
                var key = next.Substring(1);
                if (!properties.TryGetValue(key, out valueList))
                {
                    valueList = new List<string>();
                    properties[key] = valueList;

                }
            }
            else
            {
                valueList.Add(next);
            }
        }

        return new ParsedArguments(command, arguments.ToArray(), properties);
    }

    /// <summary>
    /// Determines whether the specified argument is a property name.
    /// </summary>
    /// <param name="arg">The argument.</param>
    /// <returns>
    ///   <c>true</c> if the specified argument is a property name; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsProp(string arg)
    {
        return arg.StartsWith("-");
    }
}