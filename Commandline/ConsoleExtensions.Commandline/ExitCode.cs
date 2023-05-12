namespace ConsoleExtensions.Commandline;

using System;

/// <summary>
/// Mapping from a exception or a object to a exit code.
/// </summary>
public record ExitCode(Func<object, bool> Match, int Code, string Name, string Description, ExitCodeOrder Order = ExitCodeOrder.Default);

/// <summary>
/// Order of priority for exit codes.
/// </summary>
public enum ExitCodeOrder
{
    /// <summary>
    /// The first exit codes.
    /// </summary>
    First = 0,
    
    /// <summary>
    /// The default exit codes.
    /// </summary>
    Default = 1,
    
    /// <summary>
    /// The last exit codes. Should only be used for System.Exception.
    /// </summary>
    Last = 2,

    /// <summary>
    /// The fallback for when no other exit code matches. Should only by used for 0 Success.
    /// </summary>
    Fallback = 3
}