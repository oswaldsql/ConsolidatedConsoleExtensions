namespace ConsoleExtensions.Reader;

/// <summary>
///     Builds the prompt.
/// </summary>
public class PromptBuilder
{
    /// <summary>
    ///     The lazy instance
    /// </summary>
    private static readonly Lazy<PromptBuilder> LazyInstance = new();

    /// <summary>
    ///     The converters
    /// </summary>
    private readonly Dictionary<Type, Func<string, object>> converters = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="PromptBuilder" /> class.
    /// </summary>
    public PromptBuilder()
    {
        this.AddConverter(ToTimeSpan).AddConverter(ToUri);
    }

    /// <summary>
    ///     Instances this instance.
    /// </summary>
    /// <returns></returns>
    public static PromptBuilder Instance()
    {
        return LazyInstance.Value;
    }

    /// <summary>
    ///     Gets a prompt for a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Prompter<T> ForType<T>()
    {
        var type = typeof(T);

        Func<string, T> valueConverter = s => (T) this.GetConverter(type)(s);

        return new Prompter<T> {ValueConverter = valueConverter};
    }

    /// <summary>
    ///     Gets the converter.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private Func<string, object> GetConverter(Type type)
    {
        if (this.converters.TryGetValue(type, out var converter))
        {
            return s => converter(s);
        }

        if (type.IsEnum)
        {
            return s => Enum.Parse(type, s, true);
        }

        if (typeof(IConvertible).IsAssignableFrom(type))
        {
            return s => Convert.ChangeType(s, type);
        }

        throw new Exception();
    }

    /// <summary>
    ///     Fors the type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public Prompter<object> ForType(Type type)
    {
        var valueConverter = this.GetConverter(type);

        return new Prompter<object> {ValueConverter = valueConverter};
    }

    /// <summary>
    ///     Converts to timespan.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    private static TimeSpan ToTimeSpan(string input)
    {
        return TimeSpan.Parse(input);
    }

    /// <summary>
    ///     Converts to uri.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    private static Uri ToUri(string input)
    {
        return new Uri(input);
    }

    /// <summary>
    ///     Adds the converter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="converter">The converter.</param>
    /// <returns></returns>
    public PromptBuilder AddConverter<T>(Func<string, T> converter)
    {
        this.converters[typeof(T)] = s => converter(s) ?? throw new InvalidOperationException();
        return this;
    }
}