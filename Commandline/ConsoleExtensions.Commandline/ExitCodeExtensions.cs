namespace ConsoleExtensions.Commandline;

using System;
using System.IO;
using System.Threading.Tasks;
using Exceptions;

/// <summary>
///     Extension methods for the exit codes.
/// </summary>
public static class ExitCodeExtensions
{
    /// <summary>
    ///     Adds the default exit codes.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <returns>The controller</returns>
    public static Controller AddDefaultExitCodes(this Controller controller)
    {
        controller.AddExitCode(_ => true, 0, "Success", "The command was executed successfully", ExitCodeOrder.Fallback)
            .AddExceptionExitCode<Exception>(1, "Error", "The command was executed with an unknown error", ExitCodeOrder.Last)

            .AddExceptionExitCode<TaskCanceledException>(4, "Canceled", "The command was canceled by the user")
            .AddExceptionExitCode<TimeoutException>(19, "Timeout", "The command was executed with an timeout error")
            .AddExceptionExitCode<OperationCanceledException>(7, "Operation canceled", "The command was canceled by the user")

            .AddExceptionExitCode<ArgumentException>(5, "Argument error", "The command was executed with an invalid argument")
            .AddExceptionExitCode<AggregateException>(6, "Aggregate error", "The command was executed with an aggregate error")
            .AddExceptionExitCode<NotImplementedException>(8, "Not implemented", "The command was not implemented")
            .AddExceptionExitCode<OutOfMemoryException>(13, "Out of memory", "The command was executed with an out of memory error")

            .AddExceptionExitCode<InvalidArgumentFormatException>(0, "Invalid argument format", "The command was executed with an invalid argument format")
            .AddExceptionExitCode<UnknownOptionException>(0, "Invalid option format", "The command was executed with an unknown option format")
            .AddExceptionExitCode<UnknownCommandException>(0, "Unknown command", "The command was executed with an unknown command")
            .AddExceptionExitCode<TooManyArgumentsException>(0, "Too many arguments", "The command was executed with too many arguments")
            .AddExceptionExitCode<MissingArgumentException>(0, "Missing argument", "The command was executed with a missing argument")
            .AddExceptionExitCode<InvalidParameterFormatException>(0, "Invalid parameter format", "The command was executed with an invalid parameter format");

        controller.TemplateParser.AddTypeTemplate<ExitCode[]>("[s:error]Test[/]");

        return controller;
    }

    /// <summary>
    ///     Adds a exit code based on the return value of a method.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <param name="match">The lambda to used the  match the value.</param>
    /// <param name="code">The code.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="order">The order.</param>
    /// <returns>The controller</returns>
    public static Controller AddExitCode(this Controller controller, Func<object, bool> match, int code, string name, string description = null, ExitCodeOrder order = ExitCodeOrder.Default)
    {
        controller.ExitCodes.Add(new ExitCode(match, code, name, description ?? "", order));
        return controller;
    }

    /// <summary>
    ///     Adds a exit code based on the return value of a method.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <param name="match">The lambda to used the  match the value.</param>
    /// <param name="code">The code.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="order">The order.</param>
    /// <returns>The controller</returns>
    public static Controller AddExitCode<T>(this Controller controller, Func<T, bool> match, int code, string name, string description = null, ExitCodeOrder order = ExitCodeOrder.Default)
    {
        Func<object, bool> match2 = o => o is T t && match(t);
        controller.ExitCodes.Add(new ExitCode(match2, code, name, description ?? "", order));
        return controller;
    }

    /// <summary>
    ///     Adds a exit code based on a exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controller">The controller.</param>
    /// <param name="code">The code. The return code is offset by 1000.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="order">The order.</param>
    /// <returns>The controller</returns>
    public static Controller AddExceptionExitCode<T>(this Controller controller, int code, string name, string description = null, ExitCodeOrder order = ExitCodeOrder.Default) where T : Exception
    {
        Func<object, bool> match = o => o is T;
        controller.ExitCodes.Add(new ExitCode(match, 1000 + code, name, description ?? "", order));
        return controller;
    }
}