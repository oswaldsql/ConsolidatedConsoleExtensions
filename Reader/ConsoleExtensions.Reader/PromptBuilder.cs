namespace ConsoleExtensions.Reader;

public class PromptBuilder
{
    private static readonly Lazy<PromptBuilder> instance = new();
    private readonly Dictionary<Type, Func<string, object>> converters = new();

    public PromptBuilder()
    {
        this.AddConverter(ToTimeSpan).AddConverter(ToUri);
    }

    public static PromptBuilder Instance()
    {
        return instance.Value;
    }

    public Prompter<T> ForType<T>()
    {
        var type = typeof(T);

        Func<string, T> valueConverter = s => (T) this.GetConverter(type)(s);

        return new Prompter<T> {ValueConverter = valueConverter};
    }

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

        if (type.IsAssignableTo(typeof(IConvertible)))
        {
            return s => Convert.ChangeType(s, type);
        }

        throw new Exception();
    }

    public Prompter<object> ForType(Type type)
    {
        var valueConverter = this.GetConverter(type);

        return new Prompter<object> {ValueConverter = valueConverter};
    }

    private static TimeSpan ToTimeSpan(string input)
    {
        return TimeSpan.Parse(input);
    }

    private static Uri ToUri(string input)
    {
        return new Uri(input);
    }

    public PromptBuilder AddConverter<T>(Func<string, T> converter)
    {
        this.converters[typeof(T)] = s => converter(s);
        return this;
    }
}