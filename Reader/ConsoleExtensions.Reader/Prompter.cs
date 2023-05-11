namespace ConsoleExtensions.Reader;

using System.Reflection.Metadata.Ecma335;
using Proxy;

public class Prompter<T>
{
    public string Message { get; set; } = "";
    
    public Func<T>? Default { get; set; }
    
    public Func<string, T>? ValueConverter { get; set; }

    public Func<IConsoleProxy, string> Input { get; set; } = DefaultInput;

    public Func<T, bool>? IsValid { get; set; } = default;

    public T Read(IConsoleProxy? proxy = null)
    {
        proxy ??= ConsoleProxy.Instance();

        proxy.Write(this.Message);

        do
        {
            var raw = this.Input(proxy);
        
            if (raw == "" && this.Default != null) return this.Default();

            try
            {
                var result = this.ValueConverter(raw);

                if (this.IsValid != null && !this.IsValid(result))
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
            }
        } while (Retry);

        throw new ArgumentException();
    }

    public string HelpText { get; set; } = "String must be a valid " + typeof(T).Name;

    public bool Retry { get; set; } = true;

    private static string DefaultInput(IConsoleProxy proxy)
    {
        string value = "";

        var wt = new Thread(() => proxy.ReadLine(out value));

        while (wt.IsAlive)
        {
            Console.Title += ".";
        }

        return value;

    } 
}
