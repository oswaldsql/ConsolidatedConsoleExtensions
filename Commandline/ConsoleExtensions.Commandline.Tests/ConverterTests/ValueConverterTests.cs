// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueConverterTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests.ConverterTests;

using System;
using System.Globalization;
using ConsoleExtensions.Commandline.Converters;
using Parser;
using Xunit;

public class WellKnowTypeMapperTests
{
    // Create tests for WellKnowTypeMapper
    [Theory]
    [InlineData(typeof(bool), "true")]
    [InlineData(typeof(int), "123")]
    [InlineData(typeof(byte), "123")]
    [InlineData(typeof(char), "c")]
    [InlineData(typeof(CultureInfo), "en-gb")]
    [InlineData(typeof(DateTime), "2014-01-02")]
    [InlineData(typeof(DateTimeOffset), "2014-01-02")]
    [InlineData(typeof(decimal), "123.456")]
    [InlineData(typeof(double), "123.456")]
    [InlineData(typeof(Guid), "8E4ED524-FE8D-49DF-AA6E-1EE4A004BEFF")]
    [InlineData(typeof(short), "123")]
    [InlineData(typeof(long), "123")]
    [InlineData(typeof(sbyte), "123")]
    [InlineData(typeof(float), "123.456")]
    [InlineData(typeof(TimeSpan), "01:01:01")]
    public void WellKnowTypeMapperCanConvertToWellKnowTypes(Type type, string input)
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var mapper = new WellKnowTypeMapper();
        // Act
        var result = mapper.TryConvertToValue(input, type, null, out var obj);
        // Assert
        Assert.True(result);
        //Assert.Equal(Convert.ChangeType(input, type, CultureInfo.InvariantCulture), obj);
    }

    [Fact]
    public void WellKnowTypeMapperCanConvertToBool()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var mapper = new WellKnowTypeMapper();
        // Act
        var result = mapper.TryConvertToValue("True", typeof(bool), null, out var obj);
        // Assert
        Assert.True(result);
        Assert.Equal(true, obj);
    }

    [Fact]
    public void WellKnowTypeMapperCanConvertToNullableBool()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var mapper = new WellKnowTypeMapper();
        // Act
        var result = mapper.TryConvertToValue("True", typeof(bool), null, out var obj);
        // Assert
        Assert.True(result);
        Assert.Equal(true, obj);
    }

    [Fact]
    public void WellKnowTypeMapperCanConvertToNullableInt()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var mapper = new WellKnowTypeMapper();
        // Act
        var result = mapper.TryConvertToValue("123", typeof(int), null, out var obj);
        // Assert
        Assert.True(result);
        Assert.Equal(123, obj);
    }

    [Fact]
    public void WellKnowTypeMapperCanConvertToNullableDouble()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var mapper = new WellKnowTypeMapper();
        // Act
        var result = mapper.TryConvertToValue("123.456", typeof(double), null, out var obj);
        // Assert
        Assert.True(result);
        Assert.Equal(123.456, obj);
    }
}

/// <summary>
///     Class ValueConverter.
/// </summary>
public class ValueConverterTests
{
    /// <summary>
    ///     Given a custom option when setting option with value converter then value is set.
    /// </summary>
    [Fact]
    public void GivenACustomOption_WhenSettingOptionWithValueConverter_ThenValueIsSet()
    {
        // Arrange
        var model = new Mock();
        var modelMap = ModelParser.Parse(model);

        modelMap.AddValueConverter(s => new CustomType(s), uri => uri.InternalValue.ToUpper());

        // Act
        modelMap.SetOption("CustomTypeOption", "CustomValue");

        // Assert
        Assert.Equal("CUSTOMVALUE", modelMap.GetOption("CustomTypeOption"));
        Assert.Equal("customvalue", model.CustomTypeOption.InternalValue);
    }

    /// <summary>
    ///     Given a existing converter when overwriting then the new
    ///     converter is used.
    /// </summary>
    [Fact]
    public void GivenAExistingConverter_WhenOverwriting_ThenTheNewConverterIsUsed()
    {
        // Arrange
        var model = new Mock();
        var modelMap = ModelParser.Parse(model);

        var toObjCalled = false;
        var toStringCalled = false;

        modelMap.AddValueConverter(
            s =>
            {
                toObjCalled = true;
                return int.Parse(s);
            },
            o =>
            {
                toStringCalled = true;
                return o.ToString();
            });

        // Act
        modelMap.SetOption("IntOption", "123");

        // Assert
        Assert.Equal("123", modelMap.GetOption("IntOption"));
        Assert.Equal(123, model.IntOption);
        Assert.True(toObjCalled);
        Assert.True(toStringCalled);
    }

    /// <summary>
    ///     Given the two identical converters when converting then the last
    ///     added converter should be used.
    /// </summary>
    [Fact]
    public void GivenTwoIdenticalConverters_WhenConverting_ThenTheLastAddedConverterShouldBeUsed()
    {
        // Arrange
        var model = new Mock();
        var modelMap = ModelParser.Parse(model);

        var lastConverterCalled = false;
        var firstConverterCalled = false;
        modelMap.AddValueConverter(
            s =>
            {
                firstConverterCalled = true;
                return new CustomType(s);
            },
            _ => throw new NotImplementedException());

        modelMap.AddValueConverter(
            s =>
            {
                lastConverterCalled = true;
                return new CustomType(s);
            },
            _ => throw new NotImplementedException());

        // Act
        modelMap.SetOption("CustomTypeOption", "CustomValue");

        // Assert
        Assert.False(firstConverterCalled);
        Assert.True(lastConverterCalled);
    }

    /// <summary>
    ///     Class CustomType.
    /// </summary>
    public class CustomType
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="CustomType" /> class.
        /// </summary>
        /// <param name="internalValue">
        ///     The <see langword="internal" /> value.
        /// </param>
        public CustomType(string internalValue)
        {
            InternalValue = internalValue.ToLower();
        }

        /// <summary>
        ///     Gets the <see langword="internal" /> value.
        /// </summary>
        /// <value>
        ///     The <see langword="internal" /> value.
        /// </value>
        public string InternalValue { get; }
    }

    /// <summary>
    ///     Class Mock.
    /// </summary>
    public class Mock
    {
        /// <summary>
        ///     Gets or sets the custom type option.
        /// </summary>
        /// <value>
        ///     The custom type option.
        /// </value>
        public CustomType CustomTypeOption { get; set; }

        /// <summary>
        ///     Gets or sets the <see langword="int" /> option.
        /// </summary>
        /// <value>
        ///     The <see langword="int" /> option.
        /// </value>
        public int IntOption { get; set; }
    }
}