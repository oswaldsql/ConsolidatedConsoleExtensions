// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentParserTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests.Arguments;

using System.Linq;

using Commandline.Arguments;
using Xunit;

/// <summary>
///     Class ArgumentParserTests. Tests the argument parser.
/// </summary>
public class ArgumentParserTests
{
    [Fact]
    public void NoArgumentsReturnsHelpCommand()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse();

        // Assert
        Assert.Equal("Help", actual.Command);
        Assert.Empty(actual.Arguments);
        Assert.Empty(actual.Properties);
    }

    [Fact]
    public void FirstArgumentIsTheCommand()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse("Command");

        // Assert
        Assert.Equal("Command", actual.Command);
    }

    [Fact]
    public void ArgumentsFollowingTheCommandAreParameters()
    {
        // Arrange
        
        // Act
        var actual = ArgumentParser.Parse("Command", "Parameter1","Parameter2", "-Property");

        // Assert
        Assert.Equal("Command", actual.Command);
        Assert.Equal(2, actual.Arguments.Length);
        Assert.Equal("Parameter1|Parameter2", string.Join('|', actual.Arguments));
        Assert.Contains("Property", actual.Properties.Keys);
    }

    [Fact]
    public void ArgumentsStartingWithDashAreProperties()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse("Command", "-Property");

        // Assert
        var actualProperty = Assert.Single(actual.Properties);
        Assert.Equal("Property", actualProperty.Key);
        Assert.Empty(actual.Arguments);
        Assert.Empty(actualProperty.Value);
    }

    [Fact]
    public void PropertiesCanHaveNoValues()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse("Command", "-Property1", "-Property2");

        // Assert
        var actualProperty = actual.Properties.First();
        Assert.Equal("Property1", actualProperty.Key);
        Assert.Empty(actualProperty.Value);
    }

    [Fact]
    public void PropertiesCanHaveSingleValue()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse("Command", "-Property1", "Value", "-Property2");

        // Assert
        var actualProperty = actual.Properties.First();
        Assert.Equal("Property1", actualProperty.Key);
        var propertyValue = Assert.Single(actualProperty.Value);
        Assert.Equal("Value", propertyValue);
    }

    [Fact]
    public void PropertiesCanHaveMultipleValues()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse("Command", "-Property1", "Value1", "Value2", "-Property2");

        // Assert
        var actualProperty = actual.Properties.First();
        Assert.Equal("Property1", actualProperty.Key);
        Assert.Equal(2, actualProperty.Value.Count);
        Assert.Equal("Value1|Value2", string.Join("|",actualProperty.Value));
    }

    [Fact]
    public void WhenTheSamePropertyIsSetTwiceTheValuesAreMerged()
    {
        // Arrange

        // Act
        var actual = ArgumentParser.Parse("Command", "-Property1", "Value1", "-Property2", "-Property1", "Value2");

        // Assert
        var actualProperty = actual.Properties.First();
        Assert.Equal("Property1", actualProperty.Key);
        Assert.Equal(2, actualProperty.Value.Count);
        Assert.Equal("Value1|Value2", string.Join("|",actualProperty.Value));
    }
}