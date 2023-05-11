using ConsoleExtensions.Proxy;
using ConsoleExtensions.Reader;

var proxy = ConsoleProxy.Instance();

var age = proxy.Read<int>("What is your age");
var nowDate = DateTime.Now.Date;
proxy.WriteLine($"The you were born between {nowDate.AddYears(-(age +1)).AddDays(1):D} and {nowDate.AddYears(-age):D}");

var prompt = proxy.Read<int>(config =>
{
    config.Message = "What is your age?";
    config.IsValid = i => i >= 0 && i <= 150;
    config.Default = () => 25;
    config.ValueConverter = s => int.Parse(s) * 10;
});

proxy.WriteLine($"You specified your age to be {prompt:D}");

var timeSpan = proxy.Read<TimeSpan>(config =>
{
    config.Message = "How long do you want to sleep? ";
    config.Default = () => TimeSpan.FromHours(8);
    config.IsValid = span => span > TimeSpan.FromMinutes(5) && span < TimeSpan.FromHours(8);
    config.ValueConverter = TimeSpan.Parse;
});

proxy.WriteLine($"You wanted to sleep for {timeSpan.Minutes} minutes");

var dayOfWeek = proxy.Read<DayOfWeek>("What day of the week do you want?");

proxy.WriteLine($"You wanted {dayOfWeek}");