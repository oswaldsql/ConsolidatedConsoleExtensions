# ConsoleExtensions.Reader

Call the read method with a generic argument to have the read value parsed and returned as that a type strong value.

```csharp
    using ConsoleExtensions.Commandline;

    var proxy = ConsoleProxy.Instance();

    var age = proxy.Read<int>();
```

Specify a prompt and a optional default value.

```csharp
    var age = proxy.Read<int>("What is your age? ", 25);
```

Add additional options to the read command.

```csharp
    var timeSpan = proxy.Read<TimeSpan>(config =>
    {
        config.Message = "How long do you want to sleep? ";
        config.Default = () => TimeSpan.FromHours(6);
        config.IsValid = span => span > TimeSpan.FromMinutes(5) && span < TimeSpan.FromHours(8);
        config.ValueConverter = TimeSpan.Parse;
    });
```
