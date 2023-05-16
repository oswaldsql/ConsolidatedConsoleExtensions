// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelMap.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Converters;
using Converters.Custom;
using Exceptions;
using Util;
using Validators;

/// <summary>
///     Class ModelMap. Handles the translation of command and options to methods and properties.
/// </summary>
public class ModelMap
{
    private static readonly TaskFactory TaskFactory = new(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

    /// <summary>
    ///     The value converters used to convert from string to objects and back.
    /// </summary>
    private readonly List<IValueConverter> valueConverters = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="ModelMap" /> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="commands">The commands.</param>
    public ModelMap(IEnumerable<ModelOption> options, IEnumerable<ModelCommand> commands)
    {
        this.Options = options.ToDictionary(t => t.Name, StringComparer.InvariantCultureIgnoreCase);
        this.Commands = commands.ToDictionary(t => t.Name, StringComparer.InvariantCultureIgnoreCase);

        this.AddValueConverter(
            new EnumConverter(),
            new ConvertibleConverter(),
            new IoConverter(),
            new BoolConverter(),
            new TimeSpanValueConverter());
    }

    /// <summary>
    ///     Gets the commands.
    /// </summary>
    internal Dictionary<string, ModelCommand> Commands { get; }

    /// <summary>
    ///     Gets the options.
    /// </summary>
    internal Dictionary<string, ModelOption> Options { get; }

    /// <summary>
    ///     Adds a command to the model map.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>The ModelMap.</returns>
    public void AddCommand(ModelCommand command)
    {
        this.Commands.Add(command.Name, command);
    }

    /// <summary>
    ///     Adds option to the model map.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <returns>The ModelMap.</returns>
    public void AddOption(ModelOption option)
    {
        this.Options.Add(option.Name, option);
    }

    /// <summary>
    ///     Adds the value converter.
    /// </summary>
    /// <param name="converters">The value converters.</param>
    /// <returns>The ModelMap.</returns>
    public void AddValueConverter(params IValueConverter[] converters)
    {
        foreach (var valueConverter in converters)
        {
            if (this.valueConverters.Count == 0)
            {
                this.valueConverters.Add(valueConverter);
            }
            else
            {
                var matchingPriority =
                    this.valueConverters.FindIndex(converter => converter.Priority == valueConverter.Priority);
                if (matchingPriority != -1)
                {
                    this.valueConverters.Insert(matchingPriority, valueConverter);
                }
                else
                {
                    var firstWithHigherPriority = this.valueConverters.FindIndex(
                        converter => converter.Priority > valueConverter.Priority);
                    if (firstWithHigherPriority != -1)
                    {
                        this.valueConverters.Insert(firstWithHigherPriority, valueConverter);
                    }
                    else
                    {
                        this.valueConverters.Add(valueConverter);
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Adds a custom value converter.
    /// </summary>
    /// <typeparam name="T">The type of value converter to add.</typeparam>
    /// <param name="toValue">To value.</param>
    /// <param name="toString">To string.</param>
    /// <returns>The ModelMap.</returns>
    public void AddValueConverter<T>(Func<string, T> toValue, Func<T, string> toString)
    {
        this.AddValueConverter(new CustomValueConverter<T>(toValue, toString));
    }

    /// <summary>
    /// Invokes the specified command with the given arguments
    /// </summary>
    /// <param name="command">Name of the command to execute.</param>
    /// <param name="arguments">Arguments to parse to the command.</param>
    /// <returns>The result of the command.</returns>
    public object Invoke(string command, params string[] arguments) => this.Invoke(command, CancellationToken.None, arguments);

    /// <summary>
    ///     Invokes the specified command.
    /// </summary>
    /// <param name="command">The name of the command to be invoked.</param>
    /// <param name="token">A cancellation token.</param>
    /// <param name="arguments">The arguments to be parsed to the command.</param>
    /// <returns>The result of the method as a object.</returns>
    /// <exception cref="UnknownCommandException">Thrown is the command in not known.</exception>
    /// <exception cref="TooManyArgumentsException">Thrown is too many arguments was specified.</exception>
    /// <exception cref="MissingArgumentException">Thrown is one or more arguments was missing.</exception>
    /// <exception cref="InvalidParameterFormatException">
    ///     Thrown is the specified value of a argument is not valid for that
    ///     type.
    /// </exception>
    /// <exception cref="ArgumentException">Unable to convert type</exception>
    /// <exception cref="System.ArgumentException">Thrown is the command in not known.</exception>
    public object Invoke(string command, CancellationToken token, params string[] arguments)
    {
        if (!this.Commands.TryGetValue(command, out var method))
        {
            throw new UnknownCommandException(command, this.Commands.Values);
        }

        var infos = method.Method.GetParameters();

        if (infos.Length < arguments.Length)
        {
            throw new TooManyArgumentsException(infos.Select(s => s.Name).ToArray());
        }

        var parameterList = this.ParameterList(command, infos, arguments, token);

        var result = method.Method.Invoke(method.Source, parameterList.ToArray());

        result = GetResultFromTask(token, result, method);

        return result;
    }

    private static object GetResultFromTask(CancellationToken token, object result, ModelCommand method)
    {
        if (result.GetType().IsSubclassOf(typeof(Task)))
        {
            var task = (Task) result;
            {
                TaskFactory.StartNew(() => task, token).Unwrap().GetAwaiter().GetResult();

                if (method.Method.ReturnType == typeof(Task))
                {
                    result = null;
                }
                else
                {
                    var propertyInfo = task.GetType().GetProperty("Result");
                    result = propertyInfo?.GetValue(task);
                }
            }
        }

        return result;
    }

    private IEnumerable<object> ParameterList(string command, ParameterInfo[] infos, string[] arguments,
        CancellationToken token)
    {
        for (var index = 0; index < infos.Length; index++)
        {
            var info = infos[index];

            if (arguments.Length > index)
            {
                object result;
                try
                {
                    result = this.ConvertStringToParameterValue(info, arguments[index]);
                }
                catch (Exception e)
                {
                    throw new InvalidParameterFormatException(arguments[index], info, e);
                }

                yield return result;
            }
            else if (info.ParameterType == typeof(CancellationToken))
            {
                yield return token;
            }
            else if (info.HasDefaultValue)
            {
                yield return info.DefaultValue;
            }
            else
            {
                throw new MissingArgumentException(command, info.Name, infos.Select(s => s.Name).ToArray());
            }
        }
    }

    private object ConvertStringToParameterValue(ParameterInfo info, string stringValue)
    {
        object result;
        var type = info.ParameterType;

        if (info.TryGetCustomAttribute<CustomConverterAttribute>(out var con))
        {
            result = con.ConvertToValue(stringValue, type);
        }
        else
        {
            if (this.TryGetConverter(type, out var converter))
            {
                result = converter.ConvertToValue(stringValue, type, info);
            }
            else
            {
                throw new ArgumentException("Unable to convert type");
            }
        }

        var customValidatorAttributes = info.GetCustomAttributes<CustomValidatorAttribute>();
        foreach (var validator in customValidatorAttributes)
        {
            validator.Validate(result);
        }

        return result;
    }

    /// <summary>
    ///     Gets the option.
    /// </summary>
    /// <param name="option">The option name.</param>
    /// <returns>Zero or more values representing the value of the option.</returns>
    /// <exception cref="UnknownOptionException">Thrown is the requested option is unknown.</exception>
    public string GetOption(string option)
    {
        if (!this.Options.TryGetValue(option, out var p))
        {
            throw new UnknownOptionException(option, this.Options.Values);
        }

        var value = p.CurrentValue();

        var property = p.Property;
        if (property.TryGetCustomAttribute<CustomConverterAttribute>(out var con))
        {
            return con.ConvertToString(value);
        }

        if(this.TryGetConverter(p.Property.PropertyType, out var converter))
        {
            return converter.ConvertToString(value, property);
        }

        throw new ArgumentException("Unable to convert type");
    }

    /// <summary>
    ///     Sets the option.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="InvalidArgumentFormatException">Thrown if the option can not be converted.</exception>
    /// <exception cref="UnknownOptionException">Thrown if the option is unknown.</exception>
    public void SetOption(string option, string value)
    {
        if (this.Options.TryGetValue(option, out var p))
        {
            try
            {
                object result;

                if (p.Property.TryGetCustomAttribute<CustomConverterAttribute>(out var con))
                {
                    result = con.ConvertToValue(value, p.Property.PropertyType);
                }
                else
                {
                    if (!this.TryGetConverter(p.Property.PropertyType, out var converter))
                    {
                        throw new ArgumentException("Unable to convert type");
                    }

                    result = converter.ConvertToValue(value, p.Property.PropertyType, p.Property);
                }

                var customValidatorAttributes = ((ICustomAttributeProvider) p.Property).GetCustomAttributes<CustomValidatorAttribute>();
                foreach (var validator in customValidatorAttributes)
                {
                    validator.Validate(result);
                }

                p.Set(result);
            }
            catch (Exception e)
            {
                throw new InvalidArgumentFormatException(value, p.Property, e);
            }
        }
        else
        {
            throw new UnknownOptionException(option, this.Options.Values);
        }
    }

    private bool TryGetConverter(Type type, out IValueConverter result)
    {
        result = this.valueConverters.FirstOrDefault(converter => converter.CanConvert(type));
        return result != null;
    }
}