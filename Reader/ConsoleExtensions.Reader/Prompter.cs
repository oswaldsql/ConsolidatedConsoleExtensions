namespace ConsoleExtensions.Reader;

using Proxy;

/// <summary>
/// Prompt for getting a value from the console.
/// </summary>
/// <typeparam name="T">The type of value to get.</typeparam>
public class Prompter<T>
{
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// Gets or sets the default value to return if no value is provided.
    /// </summary>
    public Func<T>? Default { get; set; }

    /// <summary>
    /// Gets or sets the value converter.
    /// </summary>
    public Func<string, T> ValueConverter { get; set; } = _ => throw new UnknownTypeException(typeof(T));

    /// <summary>
    /// Gets or sets the input provider.
    /// </summary>
    public Func<IConsoleProxy, string> InputProvider { get; set; } = DefaultInputProvider;

    /// <summary>
    /// Gets or sets the validation provider.
    /// </summary>
    public Func<T, bool>? ValidationProvider { get; set; } = default;

    /// <summary>
    /// Gets or sets the help text provided is the value is not valid..
    /// </summary>
    public string HelpText { get; set; } = "String must be a valid " + typeof(T).Name;

    /// <summary>
    /// Gets or sets a value indicating whether retry is allowed if the user provides a invalid value.
    /// </summary>
    public bool Retry { get; set; } = false;

    /// <summary>
    /// Reads the value from the specified proxy input stream.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <returns>The value converted to the type T</returns>
    /// <exception cref="System.ArgumentException"></exception>
    public T Read(IConsoleProxy? proxy = null)
    {
        proxy ??= ConsoleProxy.Instance();

        proxy.Write(this.Message);

        do
        {
            var raw = this.InputProvider(proxy);

            if (raw == "" && this.Default != null)
            {
                return this.Default();
            }

            try
            {
                var result = this.ValueConverter(raw);

                if (result != null && this.ValidationProvider != null && !this.ValidationProvider(result))
                {
                    proxy.WriteLine(this.HelpText, ConsoleStyle.Error);
                }
                else
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                proxy.WriteLine(e.Message, ConsoleStyle.Error);
                throw;
            }
        } while (this.Retry);

        throw new ArgumentException();
    }

    /// <summary>
    /// The Default input provider.
    /// </summary>
    /// <param name="proxy">The proxy.</param>
    /// <returns>A string read from the proxy.</returns>
    private static string DefaultInputProvider(IConsoleProxy proxy)
    {
        var value = "";

        var wt = new Thread(() => proxy.ReadLine(out value));

        wt.Start();

        while (wt.IsAlive)
        {
            Console.Title += ".";
        }

        return value;
    }
}