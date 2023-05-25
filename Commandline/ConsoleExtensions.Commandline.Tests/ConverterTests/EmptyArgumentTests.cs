namespace ConsoleExtensions.Commandline.Tests.ConverterTests;

using System;
using Converters;
using Xunit;

public class EmptyArgumentTests
{
    // Empty arguments are mapped to true when set on a bool
    // Empty arguments returns false when set on a none bool value

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
}

public class EnumArgumentTests
{
    // Single enums are mapped
    // Flags enums are mapped
    // Empty args are mapped to the default value.

    [Fact]
    public void FactMethodName()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("monday", typeof(DayOfWeek), null, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var monday = Assert.IsType<DayOfWeek>(result);
        Assert.Equal(DayOfWeek.Monday, monday);
    }
}