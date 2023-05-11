namespace ConsoleExtensions.Reader;

using System.Reflection;
using ConsoleExtensions.Proxy;

public static class ReaderExtensions
{
    public static T Read<T>(this IConsoleProxy proxy, string message = "", Func<T>? defaultValue = default)
    {
        var prompter = PromptBuilder.Instance().ForType<T>();
        prompter.Message = message;
        prompter.Default = defaultValue;
        return prompter.Read(proxy);
    }
    
    public static T Read<T>(this IConsoleProxy proxy, Action<Prompter<T>> config)
    {
        var prompter = PromptBuilder.Instance().ForType<T>();
        config(prompter);
        return prompter.Read(proxy);
    }
    
    public static object Read(this IConsoleProxy proxy, Type type, string message = "", Func<object>? defaultValue = default)
    {
        var prompter = PromptBuilder.Instance().ForType(type);
        prompter.Message = message;
        prompter.Default = defaultValue;
        return prompter.Read(proxy);
    }
    
    public static object Read(this IConsoleProxy proxy, ParameterInfo parameter)
    {
        var prompter = PromptBuilder.Instance().ForType(parameter.ParameterType);
        prompter.Message = parameter.Name ?? "";
        if (parameter.DefaultValue != null)
        {
            prompter.Default = () => parameter.DefaultValue;
        }
        return prompter.Read(proxy);
    }
}