namespace ConsoleExtensions.Reader;

using System.ComponentModel;
using System.Reflection;
using Proxy;

/// <summary>
/// Extension methods for <see cref="IConsoleProxy" />.
/// </summary>
public static class ReaderExtensions
{
    /// <summary>
    /// Reads a value of the specified type from the proxy input stream.
    /// </summary>
    /// <typeparam name="T">Type to read.</typeparam>
    /// <param name="proxy">The proxy.</param>
    /// <param name="message">The message to use as a prompt.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>A value of the specified type.</returns>
    public static T Read<T>(this IConsoleProxy proxy, string message = "", Func<T>? defaultValue = default)
    {
        var prompter = PromptBuilder.Instance().ForType<T>();
        prompter.Message = message;
        prompter.Default = defaultValue;
        return prompter.Read(proxy);
    }

    /// <summary>
    /// Reads a value of the specified type from the proxy input stream.
    /// </summary>
    /// <typeparam name="T">Type to read.</typeparam>
    /// <param name="proxy">The proxy.</param>
    /// <param name="config">The configuration of the prompt.</param>
    /// <returns>A value of the specified type.</returns>
    public static T Read<T>(this IConsoleProxy proxy, Action<Prompter<T>> config)
    {
        var prompter = PromptBuilder.Instance().ForType<T>();
        config(prompter);
        return prompter.Read(proxy);
    }

    /// <summary>
    /// Reads a value of the specified type from the proxy input stream.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <param name="type">The type of value to return.</param>
    /// <param name="message">The message to use as a prompt.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>A value of the specified type.</returns>
    public static object Read(this IConsoleProxy proxy, Type type, string message = "",
        Func<object>? defaultValue = default)
    {
        var prompter = PromptBuilder.Instance().ForType(type);
        prompter.Message = message;
        prompter.Default = defaultValue;
        return prompter.Read(proxy);
    }

    /// <summary>
    /// Reads a value matching the specified parameter.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A value of the specified type.</returns>
    public static object Read(this IConsoleProxy proxy, ParameterInfo parameter)
    {
        var prompter = PromptBuilder.Instance().ForType(parameter.ParameterType);
        prompter.Message = parameter.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? parameter.Name;
        if (parameter.DefaultValue != null)
        {
            prompter.Default = () => parameter.DefaultValue;
        }

        return prompter.Read(proxy);
    }

    /// <summary>
    /// Reads a value matching the specified property.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <param name="property">The property.</param>
    /// <returns></returns>
    public static object Read(this IConsoleProxy proxy, PropertyInfo property)
    {
        var prompter = PromptBuilder.Instance().ForType(property.PropertyType);
        prompter.Message = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name;
        prompter.HelpText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

        var defaultValue = property.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        if (defaultValue != null)
        {
            prompter.Default = () => defaultValue;
        }

        return prompter.Read(proxy);
    }
}