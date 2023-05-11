namespace ConsoleExtensions.Reader;

/// <summary>
/// A unknown type was requested.
/// </summary>
/// <seealso cref="System.Exception" />
public class UnknownTypeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownTypeException"/> class.
    /// </summary>
    /// <param name="type">The type.</param>
    public UnknownTypeException(Type type) : base($"No values converter was provided for type {type.FullName}"){}
}