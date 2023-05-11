namespace ConsoleExtensions.Reader.Tests;

using System.Globalization;
using Proxy.TestHelpers;
using Xunit.Abstractions;

public class PromptForConvertableTypesTests
{
    private readonly ITestOutputHelper testOutputHelper;

    public PromptForConvertableTypesTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("001", 1)]
    [InlineData("100", 100)]
    [InlineData("-1", -1)]
    [InlineData("0", 0)]
    [InlineData("2147483647", 2147483647)]
    [InlineData("-2147483648", -2147483648)]
    public void GiveAPromptForInt_WhenGivenValidValue_ThenShouldReturnExpectedValue(string input, int expected)
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        var proxy = new TestProxy();
        proxy.Keys.Add(input + "\n");

        // Act
        var actual = proxy.Read<int>("Test");

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2147483648", "OverflowException")]
    [InlineData("-2147483649", "OverflowException")]
    [InlineData("1E2", "FormatException")]
    [InlineData("100.01", "FormatException")]
    [InlineData("100,01", "FormatException")]
    [InlineData("", "FormatException")]
    [InlineData("NAN", "FormatException")]
    [InlineData("Invalid", "FormatException")]
    public void GiveAPromptForInt_WhenGivenInvalidValue_ThenShouldThrowExpectedException(string input, string expected)
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        var proxy = new TestProxy();
        proxy.Keys.Add(input + "\n");

        // Act
        var actual = Record.Exception(() => proxy.Read<int>("Test"));

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual.GetType().Name);
        this.testOutputHelper.WriteLine(actual.GetType().Name);
    }

    [Fact]
    public void GivenAPromptForATimeSpan_WhenGivenAValidValue_ThenACustomConverterShouldBeUsed()
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        var proxy = new TestProxy();
        proxy.Keys.Add("10:50" + "\n");

        // Act
        var actual = proxy.Read<TimeSpan>("Test");

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(650), actual);
    }

    [Fact]
    public void GivenAPromptForATimeSpan_WhenNoValueIsGiven_ThenTheDefaultValueShouldBeReturned()
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        var proxy = new TestProxy();
        proxy.Keys.Add("" + "\n");

        // Act
        var actual = proxy.Read("Test", () => TimeSpan.FromMinutes(650));

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(650), actual);
    }

    [Theory]
    [InlineData("Monday", DayOfWeek.Monday)]
    [InlineData("monday", DayOfWeek.Monday)]
    [InlineData(" monday", DayOfWeek.Monday)]
    [InlineData("2", DayOfWeek.Tuesday)]
    [InlineData("", DayOfWeek.Sunday)]
    public void GivenAPromptForAEnum_WhenGivenAValidValue_ThenTheExpectedValueShouldBeReturned(string input,
        DayOfWeek expected)
    {
        // Arrange
        var proxy = new TestProxy();
        proxy.Keys.Add(input + "\n");

        // Act
        var actual = proxy.Read("Test", () => DayOfWeek.Sunday);

        // Assert
        Assert.Equal(expected, actual);
    }
}