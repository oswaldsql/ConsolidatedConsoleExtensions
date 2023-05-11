namespace ConsoleExtensions.Reader;

using Proxy;

public class Prompter<T>
{
    public string Message { get; set; } = "";

    public Func<T>? Default { get; set; }

    public Func<string, T> ValueConverter { get; set; } = _ => throw new UnknownTypeException(typeof(T));

    public Func<IConsoleProxy, string> Input { get; set; } = DefaultInput;

    public Func<T, bool>? IsValid { get; set; } = default;

    public string HelpText { get; set; } = "String must be a valid " + typeof(T).Name;

    public bool Retry { get; set; } = true;

    public T Read(IConsoleProxy? proxy = null)
    {
        proxy ??= ConsoleProxy.Instance();

        proxy.Write(this.Message);

        do
        {
            var raw = this.Input(proxy);

            if (raw == "" && this.Default != null)
            {
                return this.Default();
            }

            try
            {
                var result = this.ValueConverter(raw);

                if (result != null && this.IsValid != null && !this.IsValid(result))
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

    private static string DefaultInput(IConsoleProxy proxy)
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

public class UnknownTypeException : Exception
{
    public UnknownTypeException(Type type) : base($"No values converter was provided for type {type.FullName}"){}
}