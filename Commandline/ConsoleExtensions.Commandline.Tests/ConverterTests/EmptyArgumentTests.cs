namespace ConsoleExtensions.Commandline.Tests.ConverterTests;

using Converters;
using Xunit;

public class EmptyArgumentTests
{
    [Fact]
    public void WillNotMapFromAnyType()
    {
        // Arrange
        var sut = new EmptyArgumentConverter();

        // Act
        var actual = sut.TryConvertToString(true, null, out var actualString);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Null(actualString);
    }

    [Fact]
    public void WillMapEmptyToTrue()
    {
        // Arrange
        var sut = new EmptyArgumentConverter();

        // Act
        var actual = sut.TryConvertToValue(null, typeof(bool), null, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var boolean = Assert.IsType<bool>(result);
        Assert.True(boolean, "Should convert to true.");
    }

    [Fact]
    public void WillNotMapEmptyToFalse()
    {
        // Arrange
        var sut = new EmptyArgumentConverter();

        // Act
        var actual = sut.TryConvertToValue(null, typeof(int), null, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Null(result);
    }
}