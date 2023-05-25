namespace ConsoleExtensions.Commandline.Tests.ConverterTests;

using System;
using System.Text.RegularExpressions;
using Converters;
using Xunit;

public class EnumArgumentTests
{
    // Single enums are mapped by name.
    // Enums are not case sensitive
    // Enums can be mapped from their integer value 
    // Flags enums are mapped
    // Empty args are mapped to the default value.
    // Invalid args are not mapped.

    [Fact]
    public void EnumsAreMappedByName()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("Monday", typeof(DayOfWeek), null, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var monday = Assert.IsType<DayOfWeek>(result);
        Assert.Equal(DayOfWeek.Monday, monday);
    }

    [Fact]
    public void EnumsAreNotCaseSensitive()
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

    
    [Fact]
    public void EnumsCanBeMappedFromTheirIntegerValue()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("2", typeof(DayOfWeek), null, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var monday = Assert.IsType<DayOfWeek>(result);
        Assert.Equal(DayOfWeek.Tuesday, monday);
    }

    [Fact]
    public void FlagsEnumAreSeparatedUsingComma()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("Compiled, IgnoreCase", typeof(RegexOptions), null, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var monday = Assert.IsType<RegexOptions>(result);
        Assert.Equal(RegexOptions.Compiled | RegexOptions.IgnoreCase, monday);
    }

    [Fact]
    public void EmptyArgsAreMappedToDefaultValue()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue(null, typeof(DayOfWeek), null, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var monday = Assert.IsType<DayOfWeek>(result);
        Assert.Equal(default(DayOfWeek), monday);
    }

    [Fact]
    public void InvalidArgsAreNotMapped()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("NotADay", typeof(DayOfWeek), null, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Equal("", result);
    }

    [Fact]
    public void WillNotMapFromAnyType()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToString(true, null, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Equal("", result);
    }

    [Fact]
    public void WillNotMapToAnyType()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("Monday", typeof(bool), null, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Equal("", result);
    }

    [Fact]
    public void WillNotMapToAnyTypeWhenEmpty()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue(null, typeof(bool), null, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Equal("", result);
    }

    [Fact]
    public void WillNotMapToAnyTypeWhenInvalid()
    {
        // Arrange
        var sut = new EnumConverter();

        // Act
        var actual = sut.TryConvertToValue("NotADay", typeof(bool), null, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Equal("", result);
    }
}