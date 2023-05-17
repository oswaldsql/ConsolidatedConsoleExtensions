// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelParserTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests.ParserTests;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Parser;

using Xunit;

/// <summary>
///     Class ModelParserTests. Tests the model parser.
/// </summary>
public class ModelParserTests
{
    // Model methods are returned as commands.
    // Model properties are returned as options.
    // Command name are extracted from the method name.
    // Command display name are extracted from the display name attribute or method name.
    // Command description are extracted from the description attribute or null.
    // Option name are extracted from the property name.
    // Option display name are extracted from the display name attribute or property name.
    // Option description are extracted from the description attribute or null.
    // Overloaded methods are ignored.
    // Methods that are not public, constructors, implemented in object are ignored.
    // Properties that are not public, implemented in object are ignored.

    [Fact]
    public void ModelMethodsAreReturnedAsCommands()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.Equal(2, actual.Commands.Count);
        Assert.Equal(new []{"SimpleMethod", "MethodWithMetadata"}, actual.Commands.Select(t => t.Key));
    }

    [Fact]
    public void ModelPropertiesAreReturnedAsOptions()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.Equal(2, actual.Options.Count);
        Assert.Equal(new []{"SimpleOption", "OptionWithMetadata"}, actual.Options.Select(t => t.Key));
    }

    [Fact]
    public void CommandNameAreExtractedFromTheMethodName()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.Equal(2, actual.Commands.Count);
        Assert.Equal(new []{"SimpleMethod", "MethodWithMetadata"}, actual.Commands.Select(t => t.Key));
    }

    [Fact]
    public void CommandDisplayNameIsExtractedFromTheDisplayNameAttributeOrMethodName()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        var simpleMethod = actual.Commands["SimpleMethod"];
        Assert.Equal("Simple method", simpleMethod.DisplayName);

        var methodWithMetadata = actual.Commands["MethodWithMetadata"];
        Assert.Equal("Display name for command from attribute", methodWithMetadata.DisplayName);
    }

    [Fact]
    public void CommandDescriptionAreExtractedFromTheDescriptionAttributeOrNull()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        var simpleMethod = actual.Commands["SimpleMethod"];
        Assert.Null(simpleMethod.Description);

        var methodWithMetadata = actual.Commands["MethodWithMetadata"];
        Assert.Equal("Description for command from attribute", methodWithMetadata.Description);
    }

    [Fact]
    public void OptionNameAreExtractedFromTheMethodName()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.Equal(2, actual.Options.Count);
        Assert.Equal(new []{"SimpleOption", "OptionWithMetadata"}, actual.Options.Select(t => t.Key));
    }

    [Fact]
    public void OptionDisplayNameIsExtractedFromTheDisplayNameAttributeOrMethodName()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        var simpleMethod = actual.Options["SimpleOption"];
        Assert.Equal("Simple option", simpleMethod.DisplayName);

        var methodWithMetadata = actual.Options["OptionWithMetadata"];
        Assert.Equal("Display name for option from attribute", methodWithMetadata.DisplayName);
    }

    [Fact]
    public void OptionDescriptionAreExtractedFromTheDescriptionAttributeOrNull()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        var simpleMethod = actual.Options["SimpleOption"];
        Assert.Null(simpleMethod.Description);

        var methodWithMetadata = actual.Options["OptionWithMetadata"];
        Assert.Equal("Description for option from attribute", methodWithMetadata.Description);
    }

    [Fact]
    public void OverloadedMethodsAreIgnored()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.DoesNotContain("OverloadedMethod", actual.Commands.Keys);
    }

    [Fact]
    public void MethodsThatAreNotPublicConstructorsImplementedInObjectAreIgnored()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.DoesNotContain("PrivateMethod", actual.Commands.Keys);
        Assert.DoesNotContain("InternalMethod", actual.Commands.Keys);
        Assert.DoesNotContain("ToString", actual.Commands.Keys);
        Assert.DoesNotContain("Mock", actual.Commands.Keys);
    }

    [Fact]
    public void MethodsThatAreNotPublicImplementedInObjectAreIgnored()
    {
        // Arrange

        // Act
        var actual = ModelParser.Parse(new Mock());

        // Assert
        Assert.DoesNotContain("PrivateOption",actual.Options.Keys);
        Assert.DoesNotContain("InternalOption",actual.Options.Keys);
    }

    private class Mock
    {
        public Mock()
        {
        }

        public string SimpleOption { get; set; }

        [DisplayName("Display name for option from attribute")]
        [Description("Description for option from attribute")]
        public string OptionWithMetadata { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength]
        private string PrivateOption { get; set; }
        internal string InternalOption { get; set; }

        public string OverloadedMethod(string param) => null;
        public string OverloadedMethod(int param) => null;

        public string SimpleMethod() => null;

        [DisplayName("Display name for command from attribute")]
        [Description("Description for command from attribute")]
        public string MethodWithMetadata() => null;

        private string PrivateMethod() => null;

        internal string InternalMethod() => null;
    }
}